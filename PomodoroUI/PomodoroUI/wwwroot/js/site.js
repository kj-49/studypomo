
document.addEventListener("htmx:configRequest", (evt) => {
    let httpVerb = evt.detail.verb.toUpperCase();
    if (httpVerb === 'GET') return;

    let antiForgery = htmx.config.antiForgery;

    if (antiForgery) {

        // already specified on form, short circuit
        if (evt.detail.parameters[antiForgery.formFieldName])
            return;

        if (antiForgery.headerName) {
            evt.detail.headers[antiForgery.headerName]
                = antiForgery.requestToken;
        } else {
            evt.detail.parameters[antiForgery.formFieldName]
                = antiForgery.requestToken;
        }
    }
});


function closeOnSubmit(formId, modalId) {
    let form = document.getElementById(formId);

    form.addEventListener('submit', (e) => {
        let modalEl = document.getElementById(modalId);
        let modalInst = bootstrap.Modal.getInstance(modalEl);
        if (modalInst) {

            // Check form validity before submitting using jQuery validation
            if ($(e.target).valid()) {
                modalInst.hide();
            }

        } else {
            // If no instance exists, create one and then hide it
            modalInst = new bootstrap.Modal(modalElement);

            // Check form validity before submitting using jQuery validation
            if ($(e.target).valid()) {
                modalInst.hide();
            }

        }
    });
}

function toggleFullScreen() {
    if (!document.fullscreenElement) {
        document.documentElement.requestFullscreen();
    } else if (document.exitFullscreen) {
        document.exitFullscreen();
    }
}
document.addEventListener(
    "keydown",
    (e) => {
        if (e.keyCode === 36) {
            toggleFullScreen();
        }
    },
    false,
);