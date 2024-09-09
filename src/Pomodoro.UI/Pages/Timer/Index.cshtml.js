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
    currentInterval: null,
    timerType: 'pomodoro', // Default type
    pomodoroCount: 0 // Counter to track the number of completed Pomodoros
};

function init() {
    addEventListeners();
    restoreTimerState();
}

function addEventListeners() {
    const startStopButton = document.getElementById(STARTSTOP_BUTTON_ID);
    const resetButton = document.getElementById(RESET_BUTTON_ID);

    // Remove existing listeners to prevent duplicates
    startStopButton.removeEventListener('click', toggleTimer);
    startStopButton.addEventListener('click', toggleTimer);

    resetButton.removeEventListener('click', resetTimer);
    resetButton.addEventListener('click', resetTimer);

    // Add event listeners for radio button changes
    document.querySelectorAll(`input[name="${RADIO_GROUP_ID}"]`).forEach(radio => {
        radio.removeEventListener('change', handleRadioChange); // Ensure no duplicates
        radio.addEventListener('change', handleRadioChange);
    });

    closeOnSubmit('study-task-create-form', 'study-task-create-modal');
    closeOnSubmit('study-task-edit-form', 'study-task-edit-modal');

    window.addEventListener('beforeunload', handleBeforeUnload);
}

function requestNotificationPermission() {
    if (Notification.permission === 'default') {
        Notification.requestPermission().then(permission => {
            if (permission !== 'granted') {
                console.warn('Notifications permission denied.');
            }
        });
    }
}

function showNotification(message) {
    if (Notification.permission === 'granted') {
        new Notification('Timer Finished', { body: message });
    }
}

function getNotificationMessage() {
    switch (state.timerType) {
        case 'short-break':
            return 'Short break finished. Time to get back to work!';
        case 'long-break':
            return 'Long break finished. Let’s get back to being productive!';
        default:
            return 'Pomodoro finished. Time for a break!';
    }
}

function getNextTimerType() {
    // Determine the next timer type based on the current timer type and Pomodoro count
    if (state.timerType === 'pomodoro') {
        state.pomodoroCount++;
        if (state.pomodoroCount % 3 === 0) {
            return 'long-break'; // Switch to long break after every 3 Pomodoros
        } else {
            return 'short-break';
        }
    } else if (state.timerType === 'short-break') {
        return 'pomodoro';
    } else if (state.timerType === 'long-break') {
        return 'pomodoro';
    }
}

function toggleTimer() {
    if (state.timerOn) {
        stopTimer();
    } else {
        // Request notification permission when starting the timer
        requestNotificationPermission();
        startTimer();
    }
    updateButtonState();
}

function startTimer() {
    if (state.currentInterval) {
        clearInterval(state.currentInterval);
    }

    state.timerOn = true;
    const selectedRadio = document.querySelector(`input[name="${RADIO_GROUP_ID}"]:checked`);
    const minutes = parseInt(selectedRadio.value, 10);
    state.timerDuration = minutes * 60;
    state.secondsLeft = state.secondsLeft ?? state.timerDuration;
    state.timerType = selectedRadio.id; // Use the ID of the radio button to set the timer type

    if (state.secondsLeft === state.timerDuration) {
        state.secondsLeft--;
    }

    state.currentInterval = setInterval(() => {
        updateDisplay();
        if (--state.secondsLeft < 0) {
            stopTimer();
            showNotification(getNotificationMessage());
            const nextTimerType = getNextTimerType();
            setDuration(
                parseInt(document.querySelector(`input[name="${RADIO_GROUP_ID}"]#${nextTimerType}`).value, 10),
                nextTimerType
            );
            updateButtonState();
            updateDisplay();
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

function setDuration(minutes, timerType = 'pomodoro') {
    state.timerDuration = minutes * 60;
    state.timerType = timerType; // Set the timer type
    state.secondsLeft = state.secondsLeft ?? state.timerDuration;
    updateSelectedRadio(timerType); // Ensure the correct radio button is selected
    resetTimer();
}

function updateSelectedRadio(timerType) {
    document.querySelectorAll(`input[name="${RADIO_GROUP_ID}"]`).forEach(radio => {
        if (radio.id === timerType) {
            radio.checked = true;
        }
    });
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
    const minutes = String(Math.floor(state.secondsLeft / 60));
    const seconds = String(state.secondsLeft % 60).padStart(2, '0');
    setTimerText(minutes, seconds);

    const progressEl = document.getElementById(PROGRESS_BAR_ID);
    const selectedRadio = document.querySelector(`input[name="${RADIO_GROUP_ID}"]:checked`);
    const progressColor = selectedRadio.dataset.color || '#ccc'; // Default color if not set
    progressEl.style.backgroundColor = progressColor; // Set progress bar color
    progressEl.style.width = `${(state.secondsLeft / state.timerDuration) * 100}%`;
}

function setTimerText(minutes, seconds) {
    document.getElementById(TIMER_ELEMENT_ID).textContent = `${minutes}:${seconds}`;
}

function getSelectedValue() {
    return document.querySelector(`input[name="${RADIO_GROUP_ID}"]:checked`).value;
}

function handleRadioChange(event) {
    const radioButton = event.target;
    const minutes = parseInt(radioButton.value, 10);
    const timerType = radioButton.id; // Use the ID to set the timer type
    setDuration(minutes, timerType);
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

            if (state.timerOn != null && state.secondsLeft != null && state.timerDuration != null) {
                updateSelectedRadio(state.timerType); // Ensure the correct radio button is selected
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
