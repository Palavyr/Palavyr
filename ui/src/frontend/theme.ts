import { responsiveFontSizes, Theme } from "@material-ui/core";
import { createTheme } from "@material-ui/core/styles";

const palette = {
    background: { default: "#fff" },
    primary: {
        main: "#264B94",
    },
    secondary: {
        main: "#507FE0",
        light: "#A9D7EE",
    },
    error: {
        main: "#E6070C",
    },
    warning: {
        main: "#F2B972",
    },
    info: {
        main: "#6C78E6",
    },
    success: {
        main: "#7AE697",
    },
    tonalOffset: 0.2,
};

export const green = "#168F20";
export const red = "#8F4807";
// Colors
// #25274D
// #464866
// #AAABB8
// #2E9CCA
// #29648A

const typography = {
    fontFamily: ["'Gowun Dodum', 'Poppins'", "'Rubik'", "'Kanit'"].join(","),
    h1: {
        fontFamily: "Gowun Dodum",
        fontWeight: 400,
        fontSize: "3.052rem",
    },
    h2: {
        fontFamily: "Gowun Dodum",
        fontSize: "2.441rem",
        fontWeight: 400,
    },
    h3: {
        fontFamily: "Gowun Dodum",
        fontSize: "1.953rem",
        fontWeight: 400,
    },
    h4: {
        fontFamily: "Gowun Dodum",
        fontSize: "1.563rem",
        fontWeight: 400,
    },
    h5: {
        fontFamily: "Gowun Dodum",
        fontSize: "1.25rem",
        fontWeight: 400,
    },
    h6: {
        fontFamily: "Gowun Dodum",
        fontSize: "1rem",
        fontWeight: 400,
    },
    subtitle1: {
        fontFamily: "Gowun Dodum",
        fontSize: "1rem",
        fontWeight: 400,
    },
    subtitle2: {
        fontFamily: "Gowun Dodum",
        fontSize: "0.8rem",
        fontWeight: 400,
    },
    body1: {
        fontFamily: "Gowun Dodum",
        fontSize: ".92rem",
        fontWeight: 400,
    },
    body2: {
        fontFamily: "Gowun Dodum",
        fontSize: "0.8rem",
    },
    button: {
        fontFamily: "Gowun Dodum",
        fontSize: "0.8rem",

    },
    caption: {
        fontFamily: "Gowun Dodum",
        fontSize: "0.8rem",
    },
    overline: {
        fontFamily: "Gowun Dodum",
        fontSize: "0.8rem",
    },
};

const overrides = {
    MuiCssBaseline: {
        "@global": {
            "*": {
                "scrollbar-width": "thin",
            },
            "*::-webkit-scrollbar": {
                width: "0px",
                height: "0px",
                backgroundColor: "#25274d",
            },
        },
    },
};

// breakpoints
const xl = 1920;
const lg = 1280;
const md = 960;
const sm = 600;
const xs = 0;

const breakpoints = {
    values: {
        xl,
        lg,
        md,
        sm,
        xs,
    },
};

const theme: Theme = createTheme({
    palette,
    typography,
    breakpoints,
    overrides,
});

export default responsiveFontSizes(theme);
