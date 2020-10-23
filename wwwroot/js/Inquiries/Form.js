(() => {
    'use strict';

    document.addEventListener('DOMContentLoaded', () => {
        InquiryFormHelper.init();
        InquiryRelationHelper.init();
        InquiryRelationHelper.init();
    });

    class InquiryFormHelper {
        static init() {
            this._unknownTelephoneNumberCheckboxEventListener();
            this._inquirySearchModelOpenButtonEventListener();
            this._clearRelation();
            this._clearSearchModalInput();
            this._initUnkwonTelephoneNumberOnCheck();
        }

        static _unknownTelephoneNumberCheckboxEventListener() {
            const unknownCheckbox = document.querySelector("#unknown-telephone-number");
            unknownCheckbox.addEventListener('change', () => {
                const isUnknownCheckboxChecked = document.querySelector("#unknown-telephone-number").checked;
                const telephoneNumber = document.querySelector("#inquiry-input-form .telephone-number");

                if (isUnknownCheckboxChecked) {
                    telephoneNumber.value = "9999-99-9999"; // 電話番号を聞き取れなかった、もしくは取得できなかった場合に設定する。
                } else if (!isUnknownCheckboxChecked && telephoneNumber.value === "9999-99-9999") {
                    telephoneNumber.value = "";
                }
            });
        }

        static _inquirySearchModelOpenButtonEventListener() {
            const modalOpenButton = document.querySelector("#inquiry-search-modal-open-button");
            modalOpenButton.addEventListener('click', () => {
                InquiryRelationHelper._rowRemove();
                const tel = document.querySelector("#inquiry-input-form .telephone-number").value;
                document.querySelector("#modal-inquiry-search .inquiry-search-form .inquiry-search-telephone-number").value = tel;
            });
        }

        static _clearRelation() {
            const clearButton = document.querySelector("#inquiry-relation-clear-button");
            clearButton.addEventListener('click', () => {
                document.querySelector("#inquiry-input-form input.inquiry-relation").value = '';
                document.querySelector("#inquiry-relation-hidden").value = "";
            });
        }

        static _clearSearchModalInput() {
            const clearButton = document.querySelector("#modal-inquiry-search-clear-button");
            clearButton.addEventListener('click', () => {
                document.querySelector(".inquiry-search-form input.inquiry-search-id").value = "";
                document.querySelector(".inquiry-search-form input.inquiry-search-telephone-number").value = "";
            });
        }

        static _initUnkwonTelephoneNumberOnCheck() {
            const telephoneNumber = document.querySelector("#inquiry-input-form .telephone-number").value;
            if (telephoneNumber === "9999-99-9999") {
                const unknownCheckbox = document.querySelector("#unknown-telephone-number");
                unknownCheckbox.checked = true;
            }
        }
    }

    class InquiryRelationHelper {
        static init() {
            this._relationButtonEventListener();
        }

        static _relationButtonEventListener() {
            const relationButton = document.querySelector("#inquiry-search-button");
            relationButton.addEventListener('click', async () => {
                this._rowRemove();
                const inquiries = await this._fetchInquieries();
                if (inquiries === null) {
                    return;
                }

                this._buildInquirySearchResult(inquiries);

                return;
            })
        }

        static async _fetchInquieries() {
            const edtingInquiryId = document.querySelector("#inquiry-id-hidden").value === "" ? "-1" : document.querySelector("#inquiry-id-hidden").value;
            const id = document.querySelector(".inquiry-search-form input.inquiry-search-id").value === "" ? "-1" : document.querySelector(".inquiry-search-form input.inquiry-search-id").value;
            const telephoneNumber = document.querySelector(".inquiry-search-form input.inquiry-search-telephone-number").value;
            return await fetch(`/api/InquiryRest/${edtingInquiryId}/${id}/telephoneNumber/${telephoneNumber}`, {
                method: 'GET'
            }).then((responce) => {
                if (responce.ok) {
                   return responce.json()
                }
            });
        }

        static _buildInquirySearchResult(inquiries) {
            const dummyRow = document.querySelector('#modal-inquiry-search-result tr.dummy-row');
            const tbody = document.querySelector('#modal-inquiry-search-result tbody');

            [...inquiries].forEach(inquiry => {
                const tr = tbody.insertRow();

                const id = `#inquiry_${inquiry.id}`;

                //重複の削除
                const repeatId = tbody.querySelectorAll(id);
                if ([...repeatId].length > 0) {
                    return;
                }

                tr.id = `inquiry_${inquiry.id}`;

                const inquiryDatetime = dummyRow.querySelector("td.inquiry-datetime").cloneNode(true);
                inquiryDatetime.textContent = inquiry.incomingDateTimeText;
                tr.appendChild(inquiryDatetime);

                const inquiryCompanyName = dummyRow.querySelector("td.inquiry-company-name").cloneNode(true);
                inquiryCompanyName.textContent = inquiry.companyName;
                tr.appendChild(inquiryCompanyName);

                const inquirerName = dummyRow.querySelector("td.inquirer-name").cloneNode(true);
                inquirerName.textContent = inquiry.inquirerName;
                tr.appendChild(inquirerName);

                const inquiryQuestion = dummyRow.querySelector("td.inquiry-question").cloneNode(true);
                inquiryQuestion.textContent = inquiry.question;
                tr.appendChild(inquiryQuestion);         

                const inquiryAsnwer = dummyRow.querySelector("td.inquiry-answer").cloneNode(true);
                inquiryAsnwer.textContent = inquiry.answer;
                tr.appendChild(inquiryAsnwer);

                tr.addEventListener('dblclick', (e) => {
                    const row = e.target.parentElement;
                    const inquiryId = row.id.replace('inquiry_', '');
                    document.querySelector('#inquiry-relation-hidden').value = inquiryId;

                    document.querySelector('#inquiry-relation-info').value = inquiry.relationInquiryText;

                    const modalCloseButton = document.querySelector('#modal-inquiry-search .uk-modal-close');
                    modalCloseButton.click();
                })

                tbody.appendChild(tr);
            });

            return;
        }

        static _rowRemove() {
            const tbody = document.querySelector('#modal-inquiry-search-result tbody');
            const removeRows = tbody.querySelectorAll('tr:not(.dummy-row)');
            
            removeRows.forEach(removeRow => {
                removeRow.remove();
            });
        }
    }
})();