// Constants
const TIMER_ELEMENT_ID = 'timer';
const PROGRESS_BAR_ID = 'progress-bar';
const STARTSTOP_BUTTON_ID = 'startstop-button';
const RESET_BUTTON_ID = 'reset-button';
const SESSION_TIMER_STATE_KEY = 'timerState';
const RADIO_GROUP_ID = 'timer-options';
const STATS_STORAGE_KEY = 'pomodoroStats';

class PomodoroTimer {
    constructor(sendStatsCallback) {
        this.state = {
            timerOn: false,
            secondsLeft: null,
            timerDuration: null,
            timerType: 'pomodoro',
            pomodoroCount: 0
        };
        this.timerInterval = null;
        this.stats = {
            totalPomodoros: 0,
            totalFocusTime: 0,
            totalBreakTime: 0
        };
        this.sendStatsCallback = sendStatsCallback;
        this.lastTimestamp = null;
    }

    init() {
        this.addEventListeners();
        this.restoreTimerState();
        this.loadStats();
        this.updateDisplay(); // Add this line to initialize the display
    }

    addEventListeners() {
        document.getElementById(STARTSTOP_BUTTON_ID).addEventListener('click', () => this.toggleTimer());
        document.getElementById(RESET_BUTTON_ID).addEventListener('click', () => this.resetTimer());
        document.querySelectorAll(`input[name="${RADIO_GROUP_ID}"]`).forEach(radio => {
            radio.addEventListener('change', (e) => this.handleRadioChange(e));
        });
        this.closeOnSubmit('study-task-create-form', 'study-task-create-modal');
        this.closeOnSubmit('study-task-edit-form', 'study-task-edit-modal');
        window.addEventListener('beforeunload', () => {
            this.saveTimerState();
            this.saveStats();
        });
    }

    closeOnSubmit(formId, modalId) {
        const form = document.getElementById(formId);
        const modal = document.getElementById(modalId);
        if (form && modal) {
            form.addEventListener('submit', () => modal.style.display = 'none');
        }
    }

    toggleTimer() {
        this.state.timerOn ? this.stopTimer() : this.startTimer();
        this.updateButtonState();
    }

    startTimer() {
        this.state.timerOn = true;
        const selectedRadio = document.querySelector(`input[name="${RADIO_GROUP_ID}"]:checked`);
        const minutes = parseInt(selectedRadio.value, 10);
        this.state.timerDuration = minutes * 60;
        this.state.secondsLeft = this.state.secondsLeft ?? this.state.timerDuration;
        this.state.timerType = selectedRadio.id;

        this.lastTimestamp = performance.now();
        this.runTimer();
    }

    runTimer(timestamp) {
        if (!this.state.timerOn) return;

        if (timestamp) {
            const delta = (timestamp - this.lastTimestamp) / 1000;
            this.lastTimestamp = timestamp;
            this.state.secondsLeft -= delta;

            if (this.state.secondsLeft <= 0) {
                this.stopTimer();
                const nextTimerType = this.getNextTimerType();
                this.setDuration(
                    parseInt(document.querySelector(`input[name="${RADIO_GROUP_ID}"]#${nextTimerType}`).value, 10),
                    nextTimerType
                );
                this.updateButtonState();
                this.updateDisplay();
                return;
            }
        }

        this.updateDisplay();
        this.animationFrameId = requestAnimationFrame((ts) => this.runTimer(ts));
    }

    stopTimer() {
        cancelAnimationFrame(this.animationFrameId);
        this.state.timerOn = false;
        this.updateStats();
    }

    resetTimer() {
        this.stopTimer();
        this.state.secondsLeft = this.state.timerDuration;
        this.updateDisplay();
    }

    updateStats() {
        const completedTime = this.state.timerDuration - this.state.secondsLeft;
        if (this.state.timerType === 'pomodoro' && completedTime === this.state.timerDuration) {
            this.stats.totalPomodoros++;
            this.stats.totalFocusTime += this.state.timerDuration;
        } else if (this.state.timerType.includes('break')) {
            this.stats.totalBreakTime += completedTime;
        }
    }

    loadStats() {
        const savedStats = localStorage.getItem(STATS_STORAGE_KEY);
        if (savedStats) {
            this.stats = JSON.parse(savedStats);
        }
    }

    saveStats() {
        localStorage.setItem(STATS_STORAGE_KEY, JSON.stringify(this.stats));
    }

    getStats() {
        return this.stats;
    }

    sendStatsToBackend() {
        const stats = this.getStats();
        if (this.sendStatsCallback) {
            this.sendStatsCallback(stats);
        } else {
            console.log('Sending stats to backend:', stats);
        }
    }

    updateDisplay() {
        const minutes = String(Math.floor(this.state.secondsLeft / 60));
        const seconds = String(Math.floor(this.state.secondsLeft % 60)).padStart(2, '0');
        this.setTimerText(minutes, seconds);

        const progressEl = document.getElementById(PROGRESS_BAR_ID);
        const selectedRadio = document.querySelector(`input[name="${RADIO_GROUP_ID}"]:checked`);
        const progressColor = selectedRadio.dataset.color || '#ccc';
        progressEl.style.backgroundColor = progressColor;
        progressEl.style.width = `${(this.state.secondsLeft / this.state.timerDuration) * 100}%`;
    }

    setTimerText(minutes, seconds) {
        document.getElementById(TIMER_ELEMENT_ID).textContent = `${minutes}:${seconds}`;
    }

    updateButtonState() {
        const button = document.getElementById(STARTSTOP_BUTTON_ID);
        button.classList.toggle('btn-danger', this.state.timerOn);
        button.classList.toggle('btn-primary', !this.state.timerOn);
        button.textContent = this.state.timerOn ? 'Stop' : 'Start';
    }

    handleRadioChange(event) {
        const { value, id } = event.target;
        this.setDuration(parseInt(value, 10), id);
        this.updateButtonState();
        this.updateDisplay();
    }

    setDuration(minutes, timerType = 'pomodoro') {
        this.state.timerDuration = minutes * 60;
        this.state.timerType = timerType;
        this.state.secondsLeft = this.state.secondsLeft ?? this.state.timerDuration;
        this.updateSelectedRadio(timerType);
        this.resetTimer();
    }

    updateSelectedRadio(timerType) {
        document.querySelectorAll(`input[name="${RADIO_GROUP_ID}"]`).forEach(radio => {
            radio.checked = radio.id === timerType;
        });
    }

    getNextTimerType() {
        if (this.state.timerType === 'pomodoro') {
            this.state.pomodoroCount++;
            return this.state.pomodoroCount % 3 === 0 ? 'long-break' : 'short-break';
        }
        return 'pomodoro';
    }

    saveTimerState() {
        sessionStorage.setItem(SESSION_TIMER_STATE_KEY, JSON.stringify(this.state));
    }

    restoreTimerState() {
        const savedState = sessionStorage.getItem(SESSION_TIMER_STATE_KEY);
        if (savedState) {
            try {
                this.state = JSON.parse(savedState);
                if (this.state.timerOn != null && this.state.secondsLeft != null && this.state.timerDuration != null) {
                    this.updateSelectedRadio(this.state.timerType);
                    this.stopTimer();
                    this.updateDisplay();
                } else {
                    console.log('Saved state values are invalid');
                    this.initializeDefaultState();
                }
            } catch (e) {
                console.error('Error parsing saved state:', e);
                this.initializeDefaultState();
            }
        } else {
            console.log('No saved state found');
            this.initializeDefaultState();
        }
    }

    initializeDefaultState() {
        const selectedRadio = document.querySelector(`input[name="${RADIO_GROUP_ID}"]:checked`);
        this.setDuration(parseInt(selectedRadio.value, 10), selectedRadio.id);
        this.updateDisplay();
    }
}

// Helper function to get yesterday's date string
function yesterday() {
    const date = new Date();
    date.setDate(date.getDate() - 1);
    return date.toDateString();
}

document.addEventListener('DOMContentLoaded', function () {
    const pomodoroTimer = new PomodoroTimer(function (stats) {

        console.log('Sending stats to backend:', stats);

        document.getElementById('stats-input').value = JSON.stringify(stats);

        htmx.ajax('POST', '/Timer?handler=SaveStats', {
            swap: 'none',
            values: { stats: this.stats }
        });

    });
    pomodoroTimer.init();

    // Send stats every 5 minutes and when the page is about to unload
    setInterval(() => pomodoroTimer.sendStatsToBackend(), 5 * 1000);
    window.addEventListener('beforeunload', () => pomodoroTimer.sendStatsToBackend());
});