// Constants
const TIMER_ELEMENT_ID = 'timer';
const PROGRESS_BAR_ID = 'progress-bar';
const STARTSTOP_BUTTON_ID = 'startstop-button';
const RESET_BUTTON_ID = 'reset-button';
const SESSION_TIMER_STATE_KEY = 'timerState';
const RADIO_GROUP_ID = 'timer-options';

class PomodoroTimer {
    constructor() {
        this.state = {
            timerOn: false,
            secondsLeft: null,
            timerDuration: null,
            timerType: 'pomodoro',
            pomodoroCount: 0
        };
        this.timerInterval = null;
    }

    init() {
        this.addEventListeners();
        this.restoreTimerState();
    }

    addEventListeners() {
        document.getElementById(STARTSTOP_BUTTON_ID).addEventListener('click', () => this.toggleTimer());
        document.getElementById(RESET_BUTTON_ID).addEventListener('click', () => this.resetTimer());
        document.querySelectorAll(`input[name="${RADIO_GROUP_ID}"]`).forEach(radio => {
            radio.addEventListener('change', (e) => this.handleRadioChange(e));
        });
        this.closeOnSubmit('study-task-create-form', 'study-task-create-modal');
        this.closeOnSubmit('study-task-edit-form', 'study-task-edit-modal');
        window.addEventListener('beforeunload', () => this.saveTimerState());
    }

    closeOnSubmit(formId, modalId) {
        const form = document.getElementById(formId);
        const modal = document.getElementById(modalId);
        if (form && modal) {
            form.addEventListener('submit', () => modal.style.display = 'none');
        }
    }

    requestNotificationPermission() {
        if (Notification.permission === 'default') {
            const permission = Notification.requestPermission();
            if (permission !== 'granted') {
                console.warn('Notifications permission denied.');
            }
        }
    }

    showNotification(message) {
        if (Notification.permission === 'granted') {
            new Notification('Timer Finished', { body: message });
        }
    }

    getNotificationMessage() {
        const messages = {
            'short-break': 'Short break finished. Time to get back to work!',
            'long-break': "Long break finished. Let's get back to being productive!",
            'pomodoro': 'Pomodoro finished. Time for a break!'
        };
        return messages[this.state.timerType] || messages.pomodoro;
    }

    getNextTimerType() {
        if (this.state.timerType === 'pomodoro') {
            this.state.pomodoroCount++;
            return this.state.pomodoroCount % 3 === 0 ? 'long-break' : 'short-break';
        }
        return 'pomodoro';
    }

    toggleTimer() {
        this.state.timerOn ? this.stopTimer() : this.startTimer();
        this.updateButtonState();
    }

    startTimer() {
        this.requestNotificationPermission();
        this.state.timerOn = true;
        console.log("Timer started.");
        const selectedRadio = document.querySelector(`input[name="${RADIO_GROUP_ID}"]:checked`);
        const minutes = parseInt(selectedRadio.value, 10);
        this.state.timerDuration = minutes * 60;
        this.state.secondsLeft = this.state.secondsLeft ?? this.state.timerDuration;
        this.state.timerType = selectedRadio.id;

        if (this.state.secondsLeft === this.state.timerDuration) {
            this.state.secondsLeft--;
        }

        this.timerInterval = setInterval(() => {
            this.updateDisplay();
            if (--this.state.secondsLeft < 0) {
                this.stopTimer();
                this.showNotification(this.getNotificationMessage());
                const nextTimerType = this.getNextTimerType();
                this.setDuration(
                    parseInt(document.querySelector(`input[name="${RADIO_GROUP_ID}"]#${nextTimerType}`).value, 10),
                    nextTimerType
                );
                this.updateButtonState();
                this.updateDisplay();
            }
        }, 1000);

        console.log("here");
    }

    stopTimer() {
        clearInterval(this.timerInterval);
        this.state.timerOn = false;
        console.log("Timer stopped.");
    }

    resetTimer() {
        this.stopTimer();
        this.state.secondsLeft = this.state.timerDuration;
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

    updateButtonState() {
        const button = document.getElementById(STARTSTOP_BUTTON_ID);
        button.classList.toggle('btn-danger', this.state.timerOn);
        button.classList.toggle('btn-primary', !this.state.timerOn);
        button.textContent = this.state.timerOn ? 'Stop' : 'Start';
    }

    updateDisplay() {
        const minutes = String(Math.floor(this.state.secondsLeft / 60));
        const seconds = String(this.state.secondsLeft % 60).padStart(2, '0');
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

    handleRadioChange(event) {
        const { value, id } = event.target;
        this.setDuration(parseInt(value, 10), id);
        this.updateButtonState();
        this.updateDisplay();
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

// Initialize the timer
const pomodoroTimer = new PomodoroTimer();
pomodoroTimer.init();