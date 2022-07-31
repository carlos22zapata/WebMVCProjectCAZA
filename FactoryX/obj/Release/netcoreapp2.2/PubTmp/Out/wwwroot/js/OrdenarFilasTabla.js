//Sort table

function initTable(theadCell) {
    var orderNumber = -theadCell.dataset.order || 1, orderString, tableHTMLElement = theadCell;
    //switch (orderNumber) {
    //    case 1:
    //        {
    //            orderString = "🔺";
    //            break;
    //        }
    //    case -1:
    //        {
    //            orderString = "🔻";
    //            break;
    //        }
    //}
    while (tableHTMLElement.tagName != "TABLE") {
        tableHTMLElement = tableHTMLElement.parentElement;
    }
    if (!tableHTMLElement.dataset.scanned) {
        for (let localArray = [], rowIndexNumber = 0, zeroRowSpanSet = new Set(); rowIndexNumber < tableHTMLElement.tHead.rows.length; rowIndexNumber++) {
            if (!localArray[rowIndexNumber]) localArray[rowIndexNumber] = [];
            for (let cellIndexNumber = 0, localNumber = 0; cellIndexNumber < tableHTMLElement.tHead.rows[rowIndexNumber].cells.length; cellIndexNumber++) {
                while (localArray[rowIndexNumber][localNumber] || zeroRowSpanSet.has(localNumber)) localNumber++;
                tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].dataset.local = localNumber;
                if (tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].rowSpan) {
                    for (let scanRowNumber = 0; scanRowNumber < tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].rowSpan; scanRowNumber++) {
                        if (!localArray[rowIndexNumber + scanRowNumber]) localArray[rowIndexNumber + scanRowNumber] = [];
                        for (let scanColumnNumber = 0; scanColumnNumber < tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].colSpan; scanColumnNumber++) {
                            localArray[rowIndexNumber + scanRowNumber][localNumber + scanColumnNumber] = true;
                        }
                    }
                }
                else {
                    for (let scanColumnNumber = 0; scanColumnNumber < tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].colSpan; scanColumnNumber++) {
                        zeroRowSpanSet.add(localNumber + scanColumnNumber);
                    }
                }
                localNumber += tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].colSpan;
            }
        }
        tbody:
        for (let tbodyIndexNumber = 0; tbodyIndexNumber < tableHTMLElement.tBodies.length; tbodyIndexNumber++) {
            for (let rowIndexNumber = 0; rowIndexNumber < tableHTMLElement.tBodies[tbodyIndexNumber].rows.length; rowIndexNumber++) {
                for (let cellIndexNumber = 0; cellIndexNumber < tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells.length; cellIndexNumber++) {
                    if (tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].rowSpan != 1) continue tbody;
                }
            }
            tableHTMLElement.tBodies[tbodyIndexNumber].dataset.sortable = true;
        }
        tableHTMLElement.dataset.scanned = true;
    }
    for (let rowIndexNumber = 0; rowIndexNumber < tableHTMLElement.tHead.rows.length; rowIndexNumber++) {
        for (let cellIndexNumber = 0; cellIndexNumber < tableHTMLElement.tHead.rows[rowIndexNumber].cells.length; cellIndexNumber++) {
            if (tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].dataset.local == theadCell.dataset.local && tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].colSpan <= theadCell.colSpan) {
                if (!tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].dataset.order) {
                    tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].appendChild(document.createElement("font"));
                    tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].lastElementChild.color = "red";
                    tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].lastElementChild.size = 1;
                }
                else if (!+tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].dataset.order) tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].lastElementChild.hidden = false;
                tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].dataset.order = orderNumber;
                tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].lastElementChild.textContent = orderString;
            }
            else if (+tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].dataset.order) {
                tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].dataset.order = 0;
                tableHTMLElement.tHead.rows[rowIndexNumber].cells[cellIndexNumber].lastElementChild.hidden = true;
            }
        }
    }
    return tableHTMLElement;
}
/**
 * @param {Number} order - -1 or +1
 * @returns {(left:Number,right:Number)=>Number}
 * if(left>right)return +number; //+number>0
 * if(left==right)return 0;
 * if(left<right)return -number; //-number<0
 */
function getNumberComparatorFunction(order) {
    switch (order) {
        case 1:
            {
                return function (left, right) {
                    if (isNaN(left) || isNaN(right)) {
                        if (!isNaN(left)) return -1;
                        if (!isNaN(right)) return 1;
                        return 0;
                    }
                    else return left - right;
                }
            }
        case -1:
            {
                return function (left, right) {
                    if (isNaN(left) || isNaN(right)) {
                        if (!isNaN(left)) return -1;
                        if (!isNaN(right)) return 1;
                        return 0;
                    }
                    else return right - left;
                }
            }
    }
}
/**
 * @param {Number} order - -1 or +1
 * @param {String|String[]} [language=""] - IETF BCP 47 Language Tag
 * @param {Object} [options=null]
 *  - https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Collator
 * @param {Boolean} [useIntlCollator=false] - true or false
 * @returns {(left:String,right:String)=>Number}
 * if(left>right)return +number; //+number>0
 * if(left==right)return 0;
 * if(left<right)return -number; //-number<0
 */
function getStringComparatorFunction(order, language, options, useIntlCollator) {
    switch (order) {
        case 1:
            {
                if (!language) return function (left, right) {
                    return left == right ? 0 : left > right ? 1 : -1;
                }
                else if (!useIntlCollator) return function (left, right) {
                    return left.localeCompare(right, language, options);
                }
                else {
                    const intlCollator = new Intl.Collator(language, options);
                    return function (left, right) {
                        return intlCollator.compare(left, right);
                    }
                }
            }
        case -1:
            {
                if (!language) return function (left, right) {
                    return left == right ? 0 : left < right ? 1 : -1;
                }
                else if (!useIntlCollator) return function (left, right) {
                    return right.localeCompare(left, language, options);
                }
                else {
                    const intlCollator = new Intl.Collator(language, options);
                    return function (left, right) {
                        return intlCollator.compare(right, left);
                    }
                }
            }
    }
}
/**
 * @param {HTMLTableCellElement} theadCell - <table> -> <thead> -> <td> or <th>
 * @param {String|String[]} [locales=""] - IETF BCP 47 Language Tag
 * @param {Object} [options=null]
 *  - https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Collator
 */
function tableSort(theadCell, locales, options) {
    var tableComparatorFunction, tableHTMLElement = initTable(theadCell), tableLangString;
    if (!locales) {
        for (let node = tableHTMLElement; !tableLangString && node.parentNode; node = node.parentNode) {
            tableLangString = node.lang;
        }
    }
    for (let tbodyIndexNumber = 0; tbodyIndexNumber < tableHTMLElement.tBodies.length; tbodyIndexNumber++) {
        if (tableHTMLElement.tBodies[tbodyIndexNumber].dataset.sortable) {
            let rowInfoArray = [], tbodyComparatorFunction;
            if (locales) tbodyComparatorFunction = tableComparatorFunction || (tableComparatorFunction = getStringComparatorFunction(+theadCell.dataset.order, locales, options, true));
            else {
                let tbodyLangString;
                for (let node = tableHTMLElement.tBodies[tbodyIndexNumber]; !tbodyLangString && node.nodeName != "TABLE"; node = node.parentNode) {
                    tbodyLangString = node.lang;
                }
                if (tbodyLangString) tbodyComparatorFunction = getStringComparatorFunction(+theadCell.dataset.order, tbodyLangString, options);
                else tbodyComparatorFunction = tableComparatorFunction || (tableComparatorFunction = getStringComparatorFunction(+theadCell.dataset.order, tableLangString, options, true));
            }
            for (let rowIndexNumber = 0; rowIndexNumber < tableHTMLElement.tBodies[tbodyIndexNumber].rows.length; rowIndexNumber++) {
                let cellIndexNumber = 0, localNumber = 0;
                while (cellIndexNumber < tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells.length && localNumber + tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].colSpan <= theadCell.dataset.local) {
                    localNumber += tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].colSpan;
                    cellIndexNumber++;
                }
                rowInfoArray.push([tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber], { cellIndex: cellIndexNumber, local: localNumber }]);
            }
            rowInfoArray.sort(function (upRowInfo, downRowInfo) {
                let downRowCellIndexNumber = downRowInfo[1].cellIndex, downRowLocalNumber = downRowInfo[1].local, upRowCellIndexNumber = upRowInfo[1].cellIndex, upRowLocalNumber = upRowInfo[1].local;
                while (downRowCellIndexNumber < downRowInfo[0].cells.length && upRowCellIndexNumber < upRowInfo[0].cells.length) {
                    const resultNumber = tbodyComparatorFunction(upRowInfo[0].cells[upRowCellIndexNumber].innerText, downRowInfo[0].cells[downRowCellIndexNumber].innerText);
                    if (!resultNumber) {
                        const downRowNextLocalNumber = downRowLocalNumber + downRowInfo[0].cells[downRowCellIndexNumber].colSpan, upRowNextLocalNumber = upRowLocalNumber + upRowInfo[0].cells[upRowCellIndexNumber].colSpan;
                        if (Math.min(downRowNextLocalNumber, upRowNextLocalNumber) >= +theadCell.dataset.local + theadCell.colSpan) return 0;
                        if (upRowNextLocalNumber < downRowNextLocalNumber) {
                            upRowCellIndexNumber++;
                            upRowLocalNumber = upRowNextLocalNumber;
                        }
                        else if (upRowNextLocalNumber > downRowNextLocalNumber) {
                            downRowCellIndexNumber++;
                            downRowLocalNumber = downRowNextLocalNumber;
                        }
                        else {
                            upRowCellIndexNumber++;
                            downRowCellIndexNumber++;
                            upRowLocalNumber = upRowNextLocalNumber;
                            downRowLocalNumber = downRowNextLocalNumber;
                        }
                    }
                    else return resultNumber;
                }
                if (upRowCellIndexNumber < upRowInfo[0].cells.length) return -1;
                if (downRowCellIndexNumber < downRowInfo[0].cells.length) return 1;
                return 0;
            });
            rowInfoArray.forEach(function (rowInfo) {
                tableHTMLElement.tBodies[tbodyIndexNumber].appendChild(rowInfo[0]);
            });
        }
    }
}

// Alfanumerico
"use strict";
/**
 * @param {String} [toCase=""] - "Lower" or "Upper"
 * @param {String|String[]} [language=""] - IETF BCP 47 Language Tag
 * @returns {(string:String)=>String}
 * switch(toCase)
 * {
 *     case "Lower":return string.toLowerCase();
 *     case "Upper":return string.toUpperCase();
 *     default:return string.toString();
 * }
 */
function getStringToCaseFunction(toCase, language) {
    switch (toCase ? toCase.toLowerCase() : "") {
        case "lower":
            {
                if (language) return function (string) {
                    return string.toLocaleLowerCase(language);
                }
                else return function (string) {
                    return string.toLowerCase();
                }
            }
        case "upper":
            {
                if (language) return function (string) {
                    return string.toLocaleUpperCase(language);
                }
                else return function (string) {
                    return string.toUpperCase();
                }
            }
        default:
            {
                return function (string) {
                    return string.toString();
                }
            }
    }
}
/**
 * @param {HTMLTableCellElement} theadCell - <table> -> <thead> -> <td> or <th>
 * @param {String} [toCase=""] - "Lower" or "Upper"
 * @param {String|String[]} [locales=""] - IETF BCP 47 Language Tag
 * @param {Object} [options=null]
 *  - https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Collator
 */
function stringTableSort(theadCell, toCase, locales, options) {
    var tableComparatorFunction, tableHTMLElement = initTable(theadCell), tableLangString, tableStringToCaseFunction;
    if (!locales) {
        for (let node = tableHTMLElement; !tableLangString && node.parentNode; node = node.parentNode) {
            tableLangString = node.lang;
        }
    }
    for (let tbodyIndexNumber = 0; tbodyIndexNumber < tableHTMLElement.tBodies.length; tbodyIndexNumber++) {
        if (tableHTMLElement.tBodies[tbodyIndexNumber].dataset.sortable) {
            let rowInfoArray = [], tbodyComparatorFunction, tbodyStringToCaseFunction;
            if (locales) {
                tbodyStringToCaseFunction = tableStringToCaseFunction || (tableStringToCaseFunction = getStringToCaseFunction(toCase, locales));
                tbodyComparatorFunction = tableComparatorFunction || (tableComparatorFunction = getStringComparatorFunction(+theadCell.dataset.order, locales, options, true));
            }
            else {
                let tbodyLangString;
                for (let node = tableHTMLElement.tBodies[tbodyIndexNumber]; !tbodyLangString && node.nodeName != "TABLE"; node = node.parentNode) {
                    tbodyLangString = node.lang;
                }
                if (tbodyLangString) {
                    tbodyStringToCaseFunction = getStringToCaseFunction(toCase, tbodyLangString);
                    tbodyComparatorFunction = getStringComparatorFunction(+theadCell.dataset.order, tbodyLangString, options);
                }
                else {
                    tbodyStringToCaseFunction = tableStringToCaseFunction || (tableStringToCaseFunction = getStringToCaseFunction(toCase, tableLangString));
                    tbodyComparatorFunction = tableComparatorFunction || (tableComparatorFunction = getStringComparatorFunction(+theadCell.dataset.order, tableLangString, options, true))
                }
            }
            for (let rowIndexNumber = 0; rowIndexNumber < tableHTMLElement.tBodies[tbodyIndexNumber].rows.length; rowIndexNumber++) {
                let cellIndexNumber = 0, localNumber = 0;
                while (cellIndexNumber < tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells.length && localNumber + tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].colSpan <= theadCell.dataset.local) {
                    localNumber += tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].colSpan;
                    cellIndexNumber++;
                }
                rowInfoArray.push([tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber], { cellIndex: cellIndexNumber, local: localNumber }]);
            }
            rowInfoArray.sort(function (upRowInfo, downRowInfo) {
                let downRowCellIndexNumber = downRowInfo[1].cellIndex, downRowLocalNumber = downRowInfo[1].local, upRowCellIndexNumber = upRowInfo[1].cellIndex, upRowLocalNumber = upRowInfo[1].local;
                while (downRowCellIndexNumber < downRowInfo[0].cells.length && upRowCellIndexNumber < upRowInfo[0].cells.length) {
                    const resultNumber = tbodyComparatorFunction(tbodyStringToCaseFunction(upRowInfo[0].cells[upRowCellIndexNumber].innerText), tbodyStringToCaseFunction(downRowInfo[0].cells[downRowCellIndexNumber].innerText));
                    if (!resultNumber) {
                        const downRowNextLocalNumber = downRowLocalNumber + downRowInfo[0].cells[downRowCellIndexNumber].colSpan, upRowNextLocalNumber = upRowLocalNumber + upRowInfo[0].cells[upRowCellIndexNumber].colSpan;
                        if (Math.min(downRowNextLocalNumber, upRowNextLocalNumber) >= +theadCell.dataset.local + theadCell.colSpan) return 0;
                        if (upRowNextLocalNumber < downRowNextLocalNumber) {
                            upRowCellIndexNumber++;
                            upRowLocalNumber = upRowNextLocalNumber;
                        }
                        else if (upRowNextLocalNumber > downRowNextLocalNumber) {
                            downRowCellIndexNumber++;
                            downRowLocalNumber = downRowNextLocalNumber;
                        }
                        else {
                            upRowCellIndexNumber++;
                            downRowCellIndexNumber++;
                            upRowLocalNumber = upRowNextLocalNumber;
                            downRowLocalNumber = downRowNextLocalNumber;
                        }
                    }
                    else return resultNumber;
                }
                if (upRowCellIndexNumber < upRowInfo[0].cells.length) return -1;
                if (downRowCellIndexNumber < downRowInfo[0].cells.length) return 1;
                return 0;
            });
            rowInfoArray.forEach(function (rowInfo) {
                tableHTMLElement.tBodies[tbodyIndexNumber].appendChild(rowInfo[0]);
            });
        }
    }
}

//Numérico
"use strict";
/**
 * @param {String|String[]} [language=""] - IETF BCP 47 Language Tag
 * @param {Object} [options=null]
 *  - https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/NumberFormat
 * @param {Boolean} [useIntlCollator=false] - true or false
 * @returns {(number:Number)=>String}
 */
function getNumberFormatFunction(language, options, useIntlCollator) {
    if (!language) return function (number) {
        return number.toString();
    }
    else if (!useIntlCollator) return function (number) {
        return number.toLocaleString(language, options);
    }
    else {
        const intlNumberFormat = new Intl.NumberFormat(language, options);
        return function (number) {
            return intlNumberFormat.format(number);
        }
    }
}
/**
 * @param {HTMLTableCellElement} theadCell - <table> -> <thead> -> <td> or <th>
 * @param {Boolean} [setTitle=false] - true or false
 * @param {String|String[]} [locales=""] - IETF BCP 47 Language Tag
 * @param {Object} [options=null]
 *  - https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Collator
 *  & https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/NumberFormat
 */
function numberTableSort(theadCell, setTitle, locales, options) {
    var tableFormatFunction, tableHTMLElement = initTable(theadCell), tableNumberComparatorFunction = getNumberComparatorFunction(+theadCell.dataset.order), tableStringComparatorFunction, tableLangString;
    if (!locales) {
        for (let node = tableHTMLElement; !tableLangString && node.parentNode; node = node.parentNode) {
            tableLangString = node.lang;
        }
    }
    for (let tbodyIndexNumber = 0; tbodyIndexNumber < tableHTMLElement.tBodies.length; tbodyIndexNumber++) {
        let tbodyFormatFunction, tbodyLangString;
        if (tableHTMLElement.tBodies[tbodyIndexNumber].dataset.sortable) {
            let rowInfoArray = [], tbodyComparatorFunction;
            if (locales) tbodyComparatorFunction = tableStringComparatorFunction || (tableStringComparatorFunction = getStringComparatorFunction(+theadCell.dataset.order, locales, options, true));
            else {
                for (let node = tableHTMLElement.tBodies[tbodyIndexNumber]; !tbodyLangString && node.nodeName != "TABLE"; node = node.parentNode) {
                    tbodyLangString = node.lang;
                }
                if (tbodyLangString) tbodyComparatorFunction = getStringComparatorFunction(+theadCell.dataset.order, tbodyLangString, options);
                else tbodyComparatorFunction = tableStringComparatorFunction || (tableStringComparatorFunction = getStringComparatorFunction(+theadCell.dataset.order, tableLangString, options, true));
            }
            for (let rowIndexNumber = 0; rowIndexNumber < tableHTMLElement.tBodies[tbodyIndexNumber].rows.length; rowIndexNumber++) {
                let cellIndexNumber = 0, localNumber = 0;
                while (cellIndexNumber < tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells.length && localNumber + tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].colSpan <= theadCell.dataset.local) {
                    localNumber += tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].colSpan;
                    cellIndexNumber++;
                }
                rowInfoArray.push([tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber], { cellIndex: cellIndexNumber, local: localNumber }]);
            }
            if (setTitle) {
                if (locales) tbodyFormatFunction = tableFormatFunction || (tableFormatFunction = getNumberFormatFunction(locales, options, true));
                else if (tbodyLangString) tbodyFormatFunction = getNumberFormatFunction(tbodyLangString, options);
                else tbodyFormatFunction = tableFormatFunction || (tableFormatFunction = getNumberFormatFunction(tableLangString, options, true));
                rowInfoArray.forEach(function (rowInfo) {
                    for (let cellIndexNumber = rowInfo[1].cellIndex, localNumber = rowInfo[1].local; cellIndexNumber < rowInfo[0].cells.length && localNumber < +theadCell.dataset.local + theadCell.colSpan; cellIndexNumber++) {
                        if (!isNaN(rowInfo[0].cells[cellIndexNumber].innerText)) rowInfo[0].cells[cellIndexNumber].title = tbodyFormatFunction(+rowInfo[0].cells[cellIndexNumber].innerText);
                        localNumber += rowInfo[0].cells[cellIndexNumber].colSpan;
                    }
                });
            }
            rowInfoArray.sort(function (upRowInfo, downRowInfo) {
                let downRowCellIndexNumber = downRowInfo[1].cellIndex, downRowLocalNumber = downRowInfo[1].local, upRowCellIndexNumber = upRowInfo[1].cellIndex, upRowLocalNumber = upRowInfo[1].local;
                while (downRowCellIndexNumber < downRowInfo[0].cells.length && upRowCellIndexNumber < upRowInfo[0].cells.length) {
                    const resultNumber = isNaN(downRowInfo[0].cells[downRowCellIndexNumber].innerText) && isNaN(upRowInfo[0].cells[upRowCellIndexNumber].innerText) ? tbodyComparatorFunction(upRowInfo[0].cells[upRowCellIndexNumber].innerText, downRowInfo[0].cells[downRowCellIndexNumber].innerText) : tableNumberComparatorFunction(+upRowInfo[0].cells[upRowCellIndexNumber].innerText, +downRowInfo[0].cells[downRowCellIndexNumber].innerText);
                    if (!resultNumber) {
                        const downRowNextLocalNumber = downRowLocalNumber + downRowInfo[0].cells[downRowCellIndexNumber].colSpan, upRowNextLocalNumber = upRowLocalNumber + upRowInfo[0].cells[upRowCellIndexNumber].colSpan;
                        if (Math.min(downRowNextLocalNumber, upRowNextLocalNumber) >= +theadCell.dataset.local + theadCell.colSpan) return 0;
                        if (upRowNextLocalNumber < downRowNextLocalNumber) {
                            upRowCellIndexNumber++;
                            upRowLocalNumber = upRowNextLocalNumber;
                        }
                        else if (upRowNextLocalNumber > downRowNextLocalNumber) {
                            downRowCellIndexNumber++;
                            downRowLocalNumber = downRowNextLocalNumber;
                        }
                        else {
                            upRowCellIndexNumber++;
                            downRowCellIndexNumber++;
                            upRowLocalNumber = upRowNextLocalNumber;
                            downRowLocalNumber = downRowNextLocalNumber;
                        }
                    }
                    else return resultNumber;
                }
                if (upRowCellIndexNumber < upRowInfo[0].cells.length) return -1;
                if (downRowCellIndexNumber < downRowInfo[0].cells.length) return 1;
                return 0;
            });
            rowInfoArray.forEach(function (rowInfo) {
                tableHTMLElement.tBodies[tbodyIndexNumber].appendChild(rowInfo[0]);
            });
        }
        else if (setTitle) {
            if (locales) tbodyFormatFunction = tableFormatFunction || (tableFormatFunction = getNumberFormatFunction(locales, options, true));
            else {
                for (let node = tableHTMLElement.tBodies[tbodyIndexNumber]; !tbodyLangString && node.nodeName != "TABLE"; node = node.parentNode) {
                    tbodyLangString = node.lang;
                }
                if (tbodyLangString) tbodyFormatFunction = getNumberFormatFunction(tbodyLangString, options);
                else tbodyFormatFunction = tableFormatFunction || (tableFormatFunction = getNumberFormatFunction(tableLangString, options, true));
            }
            for (let localArray = [], rowIndexNumber = 0, zeroRowSpanSet = new Set(); rowIndexNumber < tableHTMLElement.tBodies[tbodyIndexNumber].rows.length; rowIndexNumber++) {
                let cellIndexNumber = 0, localNumber = 0;
                if (!localArray[rowIndexNumber]) localArray[rowIndexNumber] = [];
                while (localArray[rowIndexNumber][localNumber] || zeroRowSpanSet.has(localNumber)) localNumber++;
                while (cellIndexNumber < tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells.length && localNumber < +theadCell.dataset.local + theadCell.colSpan) {
                    if (localNumber + tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].colSpan > theadCell.dataset.local && !isNaN(tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].innerText)) tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].title = tbodyFormatFunction(+tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].innerText);
                    if (tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].rowSpan) {
                        for (let scanRowNumber = 0; scanRowNumber < tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].rowSpan; scanRowNumber++) {
                            if (!localArray[rowIndexNumber + scanRowNumber]) localArray[rowIndexNumber + scanRowNumber] = [];
                            for (let scanColumnNumber = 0; scanColumnNumber < tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].colSpan; scanColumnNumber++) {
                                localArray[rowIndexNumber + scanRowNumber][localNumber + scanColumnNumber] = true;
                            }
                        }
                    }
                    else {
                        for (let scanColumnNumber = 0; scanColumnNumber < tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].colSpan; scanColumnNumber++) {
                            zeroRowSpanSet.add(localNumber + scanColumnNumber);
                        }
                    }
                    localNumber += tableHTMLElement.tBodies[tbodyIndexNumber].rows[rowIndexNumber].cells[cellIndexNumber].colSpan;
                    while (localArray[rowIndexNumber][localNumber] || zeroRowSpanSet.has(localNumber)) localNumber++;
                    cellIndexNumber++;
                }
            }
        }
    }
    if (tableHTMLElement.tFoot && setTitle) {
        let tfootFormatFunction, tfootLangString;
        if (locales) tfootFormatFunction = tableFormatFunction || getNumberFormatFunction(locales, options);
        else {
            for (let node = tableHTMLElement.tFoot; !tfootLangString && node.nodeName != "TABLE"; node = node.parentNode) {
                tfootLangString = node.lang;
            }
            if (tfootLangString) tfootFormatFunction = getNumberFormatFunction(tfootLangString, options);
            else tfootFormatFunction = tableFormatFunction || getNumberFormatFunction(tableLangString, options);
        }
        for (let localArray = [], rowIndexNumber = 0, zeroRowSpanSet = new Set(); rowIndexNumber < tableHTMLElement.tFoot.rows.length; rowIndexNumber++) {
            let cellIndexNumber = 0, localNumber = 0;
            if (!localArray[rowIndexNumber]) localArray[rowIndexNumber] = [];
            while (localArray[rowIndexNumber][localNumber] || zeroRowSpanSet.has(localNumber)) localNumber++;
            while (cellIndexNumber < tableHTMLElement.tFoot.rows[rowIndexNumber].cells.length && localNumber < +theadCell.dataset.local + theadCell.colSpan) {
                if (localNumber + tableHTMLElement.tFoot.rows[rowIndexNumber].cells[cellIndexNumber].colSpan > theadCell.dataset.local && !isNaN(tableHTMLElement.tFoot.rows[rowIndexNumber].cells[cellIndexNumber].innerText)) tableHTMLElement.tFoot.rows[rowIndexNumber].cells[cellIndexNumber].title = tfootFormatFunction(+tableHTMLElement.tFoot.rows[rowIndexNumber].cells[cellIndexNumber].innerText);
                if (tableHTMLElement.tFoot.rows[rowIndexNumber].cells[cellIndexNumber].rowSpan) {
                    for (let scanRowNumber = 0; scanRowNumber < tableHTMLElement.tFoot.rows[rowIndexNumber].cells[cellIndexNumber].rowSpan; scanRowNumber++) {
                        if (!localArray[rowIndexNumber + scanRowNumber]) localArray[rowIndexNumber + scanRowNumber] = [];
                        for (let scanColumnNumber = 0; scanColumnNumber < tableHTMLElement.tFoot.rows[rowIndexNumber].cells[cellIndexNumber].colSpan; scanColumnNumber++) {
                            localArray[rowIndexNumber + scanRowNumber][localNumber + scanColumnNumber] = true;
                        }
                    }
                }
                else {
                    for (let scanColumnNumber = 0; scanColumnNumber < tableHTMLElement.tFoot.rows[rowIndexNumber].cells[cellIndexNumber].colSpan; scanColumnNumber++) {
                        zeroRowSpanSet.add(localNumber + scanColumnNumber);
                    }
                }
                localNumber += tableHTMLElement.tFoot.rows[rowIndexNumber].cells[cellIndexNumber].colSpan;
                while (localArray[rowIndexNumber][localNumber] || zeroRowSpanSet.has(localNumber)) localNumber++;
                cellIndexNumber++;
            }
        }
    }
}