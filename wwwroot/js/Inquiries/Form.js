(() => {
    'use strict';

    document.addEventListener('DOMContentLoaded', () => {
        InquiryFormHelper.init();
        InquiryRelationHelper.init();
        InquiryRelationHelper.init();
    });

    class InquiryFormHelper {
        static init() {
            this._inquirySearchModelOpenButtonEventListener();
            this._clearRelation();
            this._clearSearchModalInput();
        }

        static _inquirySearchModelOpenButtonEventListener() {
            const modalOpenButton = document.querySelector("#inquiry-search-modal-open-button");
            modalOpenButton.addEventListener('click', () => {
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
    }

    class InquiryRelationHelper {
        static init() {
            this._relationButtonEventListener();
        }

        static _relationButtonEventListener() {
            const relationButton = document.querySelector("#inquiry-search-button");
            relationButton.addEventListener('click', async () => {
                const inquiries = await this._fetchInquieries();
                if (inquiries === null) {
                    return;
                }
                this._buildInquirySearchResult(inquiries);

                return;
            })
        }

        static async _fetchInquieries() {
            const id = document.querySelector(".inquiry-search-form input.inquiry-search-id").value === "" ? "-1" : document.querySelector(".inquiry-search-form input.inquiry-search-id").value;
            const telephoneNumber = document.querySelector(".inquiry-search-form input.inquiry-search-telephone-number").value;

            return await fetch(`/api/InquiryRest/${id}/telephoneNumber/${telephoneNumber}`, {
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
                const tr = tbody.insertRow(1);
                const inquiryDatetime = dummyRow.querySelector("td.inquiry-datetime");
                inquiryDatetime.textContent = inquiry.incomingDate;
                tr.appendChild(inquiryDatetime);

                const inquiryCompanyName = dummyRow.querySelector("td.inquiry-company-name");
                inquiryCompanyName.textContent = inquiry.companyName;
                tr.appendChild(inquiryCompanyName);

                const inquirerName = dummyRow.querySelector("td.inquirer-name");
                inquirerName.textContent = inquiry.inquirerName;
                tr.appendChild(inquirerName);

                const inquiryQuestion = dummyRow.querySelector("td.inquiry-question");
                inquiryQuestion.textContent = inquiry.question;
                tr.appendChild(inquiryQuestion);         

                const inquiryAsnwer = dummyRow.querySelector("td.inquiry-answer");
                inquiryAsnwer.textContent = inquiry.answer;
                tr.appendChild(inquiryAsnwer);
            });

            return;
        }
    }
})();