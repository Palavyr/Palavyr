import { makeStyles } from "@material-ui/core";


export const useNavBarStyles = makeStyles({
    root: {},
    menuButton: {},
    appBar: {
        // height: "75px"
        // boxShadow: theme.shadows[6],
    },
    toolbar: {
        display: "flex",
        justifyContent: "space-between"
    },
    menuButtonText: {
        fontSize: "large"
        // fontSize: theme.typography.body1.fontSize,
        // fontWeight: theme.typography.h6.fontWeight
    },
    brandText: {
        fontWeight: "bolder",
        marginLeft: "5rem",
        color: "white"
    },
    noDecoration: {
        textDecoration: "none !important"
    }
});