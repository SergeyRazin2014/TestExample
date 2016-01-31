var ArrayHelper = {};

ArrayHelper.findItemByKeyValue = function (arraytosearch, propName, valuetosearch) {
    for (var i = 0; i < arraytosearch.length; i++) {

        if (arraytosearch[i][propName] === valuetosearch) {
            return arraytosearch[i];
        }
    }
    return null;
};

ArrayHelper.findIndexByKeyValue = function (arraytosearch, propName, valuetosearch) {

    if (!Array.isArray(arraytosearch))
        throw "Первый аоргумент должен быть массивом!";

    var isAllElemetnsHasFindedProperty = arraytosearch.every(function (element, index, array) {
        return element.hasOwnProperty(propName);
    });

    if (!isAllElemetnsHasFindedProperty)
        throw "Один из элементов массива не содержит свойства указанного в поиске."

    for (var i = 0; i < arraytosearch.length; i++) {

        if (arraytosearch[i][propName] === valuetosearch) {
            return i;
        }
    }
    return null;
};