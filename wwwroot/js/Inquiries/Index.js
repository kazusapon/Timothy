(() => {
    'use strict';

    document.addEventListener('DOMContentLoaded', () => {
        InquiryListHelper.init();
    });

    class InquiryListHelper {
        static init() {
            this._searchFormClear();
            this._inquiryListLineEventListener();
        }

        static _searchFormClear() {
            const clearButton = document.querySelector("#clear-button");
            clearButton.addEventListener('click', () => {
                const textFields = document.querySelectorAll("#inquiry-search-area .uk-input");
                textFields.forEach(textField => {
                    textField.value = '';
                });

                document.querySelector("#inquiry-search-area .uk-select").options[0].selected = true; //「すべて」を選択
                document.querySelector("#inquiry-search-area .uk-checkbox").checked = true;
            });
        }

        static _inquiryListLineEventListener() {
            const inquiryListLines = document.querySelectorAll("#inquiry-list .inquiry-list-line");

            inquiryListLines.forEach(inquiryListLine => {
                inquiryListLine.addEventListener('dblclick', () => {
                    const inquiryId = inquiryListLine.querySelector(".inquiry-id").textContent;

                    location.href = `/Inquiry/${inquiryId}`;
                })
            });
        }
    }
})();