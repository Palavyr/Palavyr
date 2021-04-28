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

function compareValues<T> (valA: T, valB: T, reverse: boolean) {

    if (valA < valB) {
        return reverse ? 1 : -1;
    }
    if (valA > valB) {
        return  reverse ? -1 : 1;
    }
    return 0;

}