import { makeStyles } from "@material-ui/core";


export const useNavBarStyles = makeStyles(theme => ({
    root: {},
    menuButton: {},
    appBar: {
        boxShadow: theme.shadows[6],
        backgroundColor: theme.palette.common.white
    },
    toolbar: {
        display: "flex",
        justifyContent: "space-between"
    },
    menuButtonText: {
        fontSize: theme.typography.body1.fontSize,
        fontWeight: theme.typography.h6.fontWeight
    },
    brandText: {
        marginLeft: "5rem"
    },
    noDecoration: {
        textDecoration: "none !important"
    }
}));