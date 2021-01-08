export const sortByPropertyAlphabetical = (stringPropertyGetter: (x: object) => string, array: Array<any> ) => {
    return array.sort((a: any, b: any) => {

    const valA = stringPropertyGetter(a);
    const valB = stringPropertyGetter(b);

    if (valA == null || valB == null) {
        return 0
    }
    var nameA = valA.toUpperCase(); // ignore upper and lowercase
    var nameB = valB.toUpperCase(); // ignore upper and lowercase
    if (nameA < nameB) {
      return -1;
    }
    if (nameA > nameB) {
      return 1;
    }

    // names must be equal
    return 0;
  })}