import { createMuiTheme, responsiveFontSizes, Theme } from "@material-ui/core";

const palette = {
    background: { default: "#F2F2F2" },
    primary: {
        main: "#264B94",
    },
    secondary: {
        main: "#507FE0",
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
    tonalOffset: 0.1,
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
    fontFamily: ["'Poppins'", "'Rubik'", "'Kanit'"].join(","),
    h1: {
        fontFamily: "Kanit",
    },
    h2: {
        fontFamily: "Kanit",
    },
    h3: {
        fontFamily: "Kanit",
    },
    h4: {
        fontFamily: "Kanit",
    },
    h5: {
        fontFamily: "Kanit",
    },
    h6: {
        fontFamily: "Kanit",
    },
    subtitle1: {
        fontFamily: "Poppins",
    },
    subtitle2: {
        fontFamily: "Poppins",
    },
    body1: {
        fontFamily: "Poppins",
    },
    body2: {
        fontFamily: "Poppins",
    },
    button: {
        fontFamily: "Poppins",
    },
    caption: {
        fontFamily: "Poppins",
    },
    overline: {
        fontFamily: "Poppins",
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

const theme: Theme = createMuiTheme({
    palette,
    typography,
    breakpoints,
    overrides,
});

export default responsiveFontSizes(theme);

// // colors
// const primary = "#97c6a3";
// const secondary = "#c6a397";
// const black = "#282630";
// const darkBlack = "rgb(36, 40, 44)";
// const background = "#54585A"; //"#efedf4";
// const warningLight = "rgba(253, 200, 69, .3)";
// const warningMain = "rgba(253, 200, 69, .5)";
// const warningDark = "rgba(253, 200, 69, .7)";

// // "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)"

// // border
// const borderWidth = 2;
// const borderColor = "rgba(0, 0, 0, 0.13)";

// // spacing
// const spacing = 8;

// // <link href="https://fonts.googleapis.com/css2?family=Rubik:ital,wght@0,400;0,500;1,600&family=Poppins:ital,wght@0,200;0,300;0,400;0,500;1,100;1,200;1,300&display=swap" rel="stylesheet">
// // "linear-gradient(354deg, rgba(1,30,109,1) 10%, rgba(0,212,255,1) 100%)"
// const theme: PalavyrTheme = createMuiTheme({
//     palette: {
//         // background: { default: "rgb(1,96,162)"},
//     },
//     typography: {
//         fontFamily: [
//             // "'Kanit'",
//             "'Rubik'",
//             "'Poppins'",
//         ],
//     },
//     overrides: {
//         MuiCssBaseline: {
//             "@global": {
//                 "*": {
//                     "scrollbar-width": "thin",
//                 },
//                 "*::-webkit-scrollbar": {
//                     width: "4px",
//                     height: "4px",
//                 },
//             },
//         },
//     },
// });

// const Backuptheme = createMuiTheme({
//     palette: {
//         primary: {
//             main: primary,
//             light: "fff",
//             wow: "ttt",
//         },
//         secondary: { main: secondary },
//         common: {
//             black,
//         },
//         warning: {
//             light: warningLight,
//             main: warningMain,
//             dark: warningDark,
//         },
//         // Used to shift a color's luminance by approximately
//         // two indexes within its tonal palette.
//         // E.g., shift from Red 500 to Red 300 or Red 700.
//         tonalOffset: 0.2,
//         background: {
//             paper: "#54585A",
//             // paper: "#E8E8E8",
//             default: background,
//         },
//         // spacing
//     },
//     breakpoints: {
//         // Define custom breakpoint values.
//         // These will apply to Material-UI components that use responsive
//         // breakpoints, such as `Grid` and `Hidden`. You can also use the
//         // theme breakpoint functions `up`, `down`, and `between` to create
//         // media queries for these breakpoints
//         values: {
//             xl,
//             lg,
//             md,
//             sm,
//             xs,
//         },
//     },
//     overrides: {
//         // border: {
//         //     borderColor: borderColor,
//         //     borderWidth: borderWidth
//         // },
//         MuiExpansionPanel: {
//             root: {
//                 position: "static",
//             },
//         },
//         MuiTableCell: {
//             root: {
//                 paddingLeft: spacing * 2,
//                 paddingRight: spacing * 2,
//                 borderBottom: `${borderWidth}px solid ${borderColor}`,
//                 [`@media (max-width:  ${sm}px)`]: {
//                     paddingLeft: spacing,
//                     paddingRight: spacing,
//                 },
//             },
//         },
//         MuiDivider: {
//             root: {
//                 backgroundColor: borderColor,
//                 height: borderWidth,
//             },
//         },
//         // MuiPrivateNotchedOutline: {
//         //     root: {
//         //         borderWidth: borderWidth
//         //     }
//         // },
//         MuiListItem: {
//             divider: {
//                 borderBottom: `${borderWidth}px solid ${borderColor}`,
//             },
//         },
//         MuiDialog: {
//             paper: {
//                 width: "100%",
//                 maxWidth: 430,
//                 marginLeft: spacing,
//                 marginRight: spacing,
//             },
//         },
//         MuiTooltip: {
//             tooltip: {
//                 backgroundColor: darkBlack,
//             },
//         },
//         MuiExpansionPanelDetails: {
//             root: {
//                 [`@media (max-width:  ${sm}px)`]: {
//                     paddingLeft: spacing,
//                     paddingRight: spacing,
//                 },
//             },
//         },
//     },
//     typography: {
//         // fontFamily: "'Quicksand', sans-serif",
//         fontFamily: "'Varela Round', sans-serif",
//         // useNextVariants: true
//     },
// });

// export default responsiveFontSizes(theme);
