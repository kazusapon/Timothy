(() => {
    'use strict';

    document.addEventListener('DOMContentLoaded', () => {
        InquiryFormHelper.init();
    });

    class InquiryFormHelper {
        static init() {
            this._clearRelation();
            this._clearSearchModalInput();
        }

        static _clearRelation() {
            const clearButton = document.querySelector("#inquiry-relation-clear-button");
            clearButton.addEventListener('click', () => {
                document.querySelector("#inquiry-input-form input.inquiry-relation").value = '';
            });
        }

        static _clearSearchModalInput() {
            const clearButton = document.querySelector("#modal-inquiry-search-clear-button");
            clearButton.addEventListener('click', () => {
                document.querySelector(".inquiry-search-form input.inquiry-search-id").value = "";
                document.querySelector(".inquiry-search-form input.inquiry-search-telephone-number").value = "";
            });
        }
    }
})();