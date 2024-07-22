const TIMER_ELEMENT_ID = 'timer';
const PROGRESS_BAR_ID = 'progress-bar';
const STARTSTOP_BUTTON_ID = 'startstop-button';
const RESET_BUTTON_ID = 'reset-button';
const SESSION_TIMER_STATE_KEY = 'timerState';
const RADIO_GROUP_ID = 'timer-options';

let state = {
    timerOn: false,
    secondsLeft: null,
    timerDuration: null,
    currentInterval: null
}

function init() {
    addEventListeners();
    restoreTimerState();
}

function addEventListeners() {
    document.getElementById(STARTSTOP_BUTTON_ID).addEventListener('click', toggleTimer);
    document.getElementById(RESET_BUTTON_ID).addEventListener('click', resetTimer);
    document.querySelectorAll(`input[name="${RADIO_GROUP_ID}"]`).forEach(radio => {
        radio.addEventListener('change', function () {
            handleTimerOptionChange(this);
        });
    });

    closeOnSubmit('study-task-create-form', 'study-task-create-modal');
    closeOnSubmit('study-task-edit-form', 'study-task-edit-modal');

    window.addEventListener('beforeunload', handleBeforeUnload);
}


function toggleTimer() {
    if (state.timerOn) {
        stopTimer();
    } else {
        startTimer();
    }
    updateButtonState();
}


function startTimer() {
    if (state.currentInterval) {
        clearInterval(state.currentInterval);
    }

    state.timerOn = true;
    const minutes = parseInt(document.querySelector(`input[name="${RADIO_GROUP_ID}"]:checked`).value, 10);
    state.timerDuration = minutes * 60;
    state.secondsLeft = state.secondsLeft ?? state.timerDuration;

    if (state.secondsLeft == state.timerDuration) {
        state.secondsLeft--;
    }

    state.currentInterval = setInterval(() => {
        updateDisplay();
        if (--state.secondsLeft < 0) {
            stopTimer();
        }
    }, 1000);
}


function stopTimer() {
    clearInterval(state.currentInterval);
    state.timerOn = false;
}


function resetTimer() {
    stopTimer();
    state.secondsLeft = state.timerDuration;
    updateDisplay();
}

function setDuration(minutes) {
    state.timerDuration = minutes * 60;
    resetTimer();
    return;
}

function updateButtonState() {
    const button = document.getElementById(STARTSTOP_BUTTON_ID);

    if (state.timerOn) {
        button.classList.add('btn-danger');
        button.classList.remove('btn-primary');
        button.textContent = 'Stop';
    } else {
        button.classList.remove('btn-danger');
        button.classList.add('btn-primary');
        button.textContent = 'Start';
    }
}


function updateDisplay() {
    const minutes = String(Math.floor(state.secondsLeft / 60)).padStart(2, '0');
    const seconds = String(state.secondsLeft % 60).padStart(2, '0');
    setTimerText(minutes, seconds);

    const progressEl = document.getElementById(PROGRESS_BAR_ID);
    progressEl.style.width = `${(state.secondsLeft / state.timerDuration) * 100}%`;
}


function setTimerText(minutes, seconds) {
    document.getElementById(TIMER_ELEMENT_ID).textContent = `${minutes}:${seconds}`;
}


function getSelectedValue() {
    return document.querySelector('input[name="timer-options"]:checked').value;
}


function handleTimerOptionChange(radioButton) {
    setDuration(radioButton.value);
    updateButtonState();
    updateDisplay();
}


function handleBeforeUnload(event) {
    saveTimerState(state);
}


function saveTimerState(state) {
    sessionStorage.setItem(SESSION_TIMER_STATE_KEY, JSON.stringify(state));
}


function restoreTimerState() {
    const savedState = sessionStorage.getItem(SESSION_TIMER_STATE_KEY);
    if (savedState) {
        try {
            state = JSON.parse(savedState);

            console.log('Parsed state:', JSON.parse(savedState));

            // Check if values are valid
            if (state.timerOn != null && state.secondsLeft != null && state.timerDuration != null) {

                // Select the correct radio button based on timerDuration
                document.querySelectorAll(`input[name="${RADIO_GROUP_ID}"]`).forEach(radio => {
                    if (parseInt(radio.value, 10) * 60 === state.timerDuration) {
                        radio.checked = true;
                    }
                });

                stopTimer();

                updateDisplay();

            } else {
                console.log('Saved state values are invalid');
                let minutes = getSelectedValue();
                setDuration(minutes);
                updateDisplay();
            }
        } catch (e) {
            console.error('Error parsing saved state:', e); 
        }
    } else {
        console.log('No saved state found');
        let minutes = getSelectedValue();
        setDuration(minutes);
        updateDisplay();
    }
}


init();
