// Enums
export enum NodeTypeCodeEnum {
    I = 0,
    II = 1,
    III = 2,
    IV = 3,
    V = 4,
    VI = 5, // Anabranch
    VII = 6, // Loopback Anchor
    VIII = 7, // Loopback terminal
    IX = 8, // Image Node Type
    X = 9, // Multioption with frozen set and not editable (used with PricingStrategy nodes like select category and nested categories
    XI = 10, // Where you set the value options given by the nodeOption (pricing strategy node)
}

export enum UnitGroups {
    Length = "length",
    intent= "area",
    Weight = "weight",
    Currency = "currency",
}
export enum UnitPrettyNames {
    Meter = "m",
    Foot = "ft",
    SquareMeters = "m^2",
    SquareFeet = "f^2",
    Grams = "g",
    KiloGrams = "kg",
    Pounds = "lbs",
    Tons = "tons",
}

export enum UnitIdEnum {
    Currency = 0,
    Meter = 1,
    Foot = 2,
    SquareMeters = 3,
    SquareFeet = 4,
    Grams = 5,
    KiloGrams = 6,
    Pounds = 7,
    Tons = 8,
}

export enum GeneralSettingsLoc {
    email,
    companyName,
    phoneNumber,
    companyLogo,
    locale,
    default_email_template,
    password,
    deleteaccount,
}

export enum PurchaseTypes {
    Free = "Free",
    Lyte = "Lyte",
    Premium = "Premium",
    Pro = "Pro",
}

export enum Interval {
    free = "free",
    monthly = "month",
    yearly = "year",
}
