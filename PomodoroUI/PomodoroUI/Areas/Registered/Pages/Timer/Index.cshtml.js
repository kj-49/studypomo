const TIMER_ELEMENT_ID = 'timer';
const PROGRESS_BAR_ID = 'progress-bar';
const STARTSTOP_BUTTON_ID = 'startstop-button';
const RESET_BUTTON_ID = 'reset-button';
let timerOn = false;
let secondsLeft = null;
let timerDuration = null;
let currentInterval = null;

function init() {
    addEventListeners();
    addSquares();
}

function addEventListeners() {

    document.getElementById(STARTSTOP_BUTTON_ID).addEventListener('click', function () {
        console.log('timeron: ', timerOn);
        let button = document.getElementById(STARTSTOP_BUTTON_ID);
        button.classList.toggle('btn-primary');
        button.classList.toggle('btn-danger');

        if (!timerOn) { // Start timer

            // Hide reset button.
            document.getElementById(RESET_BUTTON_ID).classList.add('d-none');

            button.textContent = 'Stop';
            let minutes = document.querySelector('input[name="timer-options"]:checked').value;
            timerDuration = minutes * 60;
            secondsLeft = secondsLeft ?? minutes * 60;
            console.log('sec',secondsLeft);
            startTimer(function () {
                
            });
            
        } else { // Timer stopped

            // Show reset button.
            document.getElementById(RESET_BUTTON_ID).classList.remove('d-none');

            clearInterval(currentInterval);
            timerOn = false;
            button.textContent = 'Start';
        }

    });

    document.getElementById(RESET_BUTTON_ID).addEventListener('click', function () {
        resetTimer();
    });

    let radios = document.querySelectorAll('input[name="timer-options"]');
    radios.forEach(radio => {
        radio.addEventListener('change', function () {
            resetTimer();
            setTimerText(getSelectedValue(), 0);
        });
    });

    closeOnSubmit('study-task-create-form', 'study-task-create-modal');
    closeOnSubmit('study-task-edit-form', 'study-task-edit-modal');

}


function startTimer(onTimerFinished) {
    timerOn = true;
    let minutes;
    let seconds;
    let progressEl = document.getElementById(PROGRESS_BAR_ID);
    currentInterval = setInterval(function () {
        minutes = String(parseInt(secondsLeft / 60, 10)).padStart(2, '0');
        seconds = String(parseInt(secondsLeft % 60, 10)).padStart(2, '0');

        setTimerText(minutes, seconds);

        progressEl.style.width = `${(secondsLeft/timerDuration)*100}%`;

        if (--secondsLeft < 0) {
            clearInterval(currentInterval);
            timerOn = false;
            onTimerFinished();
        }
    }, 1000);
}

function resetTimer() {
    clearInterval(currentInterval);
    secondsLeft = null;
    timerDuration = null;
    setTimerText(getSelectedValue(), 0);
}

function setTimerText(minutes, seconds) {
    let timerEl = document.getElementById(TIMER_ELEMENT_ID);

    timerEl.textContent = String(minutes).padStart(2, '0') + ":" + String(seconds).padStart(2, '0');
}

function addSquares() {
    const squares = document.querySelector('.squares');
    for (var i = 1; i < 365; i++) {
        const level = Math.floor(Math.random() * 3);
        squares.insertAdjacentHTML('beforeend', `<li data-level="${level}"></li>`);
    }
}

function getSelectedValue() {
    return document.querySelector('input[name="timer-options"]:checked').value;
}

function submitCreateStudyTaskForm() {

    let form = document.getElementById('study-task-create-form');

    let selectList = document.createElement('select');
    selectList.type = 'hidden';
    selectList.name = 'StudyTaskCreate.StudyLabelIds';

    this.selectedLabels.forEach(id => {
        const option = document.createElement('option');
        input.value = id;
        selectList.appendChild(input);
    });

    form.appendChild(selectList);
    form.submit();

}