
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
