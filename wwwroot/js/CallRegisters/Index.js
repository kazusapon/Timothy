(() => {
    'use strict';

    document.addEventListener('DOMContentLoaded', () => {
        CallRegisterHelper.init();
    });

    class CallRegisterHelper {
        static init() {
            this._callRegisterDeleteEventListner();
        }

        static _callRegisterDeleteEventListner() {
            const deleteButtons = document.querySelectorAll("#call-registers .uk-button-danger");

            deleteButtons.forEach(deleteButton => {
                deleteButton.addEventListener('click', () => {
                    const deleteButtonParent = deleteButton.parentElement;
                    const callRegisterId = deleteButtonParent.querySelector("input[type=hidden]").value;

                    document.querySelector("#delete-target-id").value = callRegisterId;
                })
            });
        }
    }
})();