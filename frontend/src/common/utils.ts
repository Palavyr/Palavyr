import { cloneDeep } from "lodash";

export const removeByIndex = (array: Array<any>, index: number) => {
    var newArray = cloneDeep(array);
    newArray.splice(index, 1);
    return newArray;
};

export const removeByItem = (array: Array<any>, item: any) => {
    var newArray = cloneDeep(array);
    newArray.splice(newArray.indexOf(item), 1);
    return newArray;
};

/// Use this method if possible
export const replaceItemAtIndex = (array: Array<any>, item: any, index: number) => {
    var newArray = cloneDeep(array);
    newArray[index] = item;
    return newArray;
};

export const replaceItemByItem = (array: Array<any>, item: any) => {
    var newArray = cloneDeep(array);
    var index = array.indexOf(item);

    if (index === -1 || index === undefined) throw new Error("Could not replace item - item not found in array.");

    newArray[array.indexOf(index)] = item;
    return newArray;
};

export const noop = () => null;

export const isNullOrUndefinedOrWhitespace = (val: any) => {
    return val === null || val === undefined || val === "";
};
