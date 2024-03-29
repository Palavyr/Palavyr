import { WidgetNodeResources } from "@common/types/api/EntityResources";


export const sortByPropertyAlphabetical = (stringPropertyGetter: (x: object) => string, array: Array<any>, reverse: boolean = false) => {
    return array.sort((a: any, b: any) => {
        const valA = stringPropertyGetter(a);
        const valB = stringPropertyGetter(b);

        if (valA == null || valB == null) {
            return 0;
        }
        var nameA = valA.toUpperCase(); // ignore upper and lowercase
        var nameB = valB.toUpperCase(); // ignore upper and lowercase

        return compareValues(nameA, nameB, reverse);
    });
};

export const sortByPropertyNumeric = (numberPropertyGetter: (x: object) => number, array: Array<any>, reverse: boolean = false) => {
    return array.sort((a: any, b: any) => {
        const valA = numberPropertyGetter(a);
        const valB = numberPropertyGetter(b);

        if (valA == null || valB == null) {
            return 0;
        }
        return compareValues(valA, valB, reverse);
    });
};

export const sortArrayOfObjects = (array: Array<Object>, reverse: boolean = false) => {
    return array.sort((a: Object, b: Object) => {
        const valA = Object.keys(a)[0];
        const valB = Object.keys(b)[0];

        if (valA == null || valB == null) {
            return 0;
        }
        return compareValues(valA, valB, reverse);
    });
};

function compareValues<T>(valA: T, valB: T, reverse: boolean = false) {
    if (valA < valB) {
        return reverse ? 1 : -1;
    }
    if (valA > valB) {
        return reverse ? -1 : 1;
    }
    return 0;
}

export const sortChildrenByOptions = (children: WidgetNodeResources) => { // TODO make generic...
    return children.sort((a, b) => {
        if (a.optionPath == null || b.optionPath == null) {
            return 0;
        }
        var nameA = a.optionPath.toUpperCase(); // ignore upper and lowercase
        var nameB = b.optionPath.toUpperCase(); // ignore upper and lowercase
        return compareValues(nameA, nameB);
    });
};
