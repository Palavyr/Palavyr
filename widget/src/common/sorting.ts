export const sortByPropertyAlphabetical = (stringPropertyGetter: (x: object) => string, array: Array<any>) => {
    if (!array) return [];
    return array.sort((a: any, b: any) => {
        const valA = stringPropertyGetter(a);
        const valB = stringPropertyGetter(b);

        if (valA == null || valB == null) {
            return 0;
        }
        var nameA = valA.toLowerCase(); // ignore upper and lowercase
        var nameB = valB.toLowerCase(); // ignore upper and lowercase

        return compareValues(nameA, nameB);
    });
};

export const sortByPropertyNumeric = (numberPropertyGetter: (x: object) => number, array: Array<any>) => {
    return array.sort((a: any, b: any) => {
        const valA = numberPropertyGetter(a);
        const valB = numberPropertyGetter(b);

        if (valA == null || valB == null) {
            return 0;
        }
        return compareValues(valA, valB);
    });
};


function compareValues<T> (valA: T, valB: T) {

    if (valA < valB) {
        return -1;
    }
    if (valA > valB) {
        return 1;
    }
    return 0;

}