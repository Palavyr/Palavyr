import { makeStyles } from "@material-ui/core/styles";

const drawerWidth: number = 240;
export const useDashboardStyles = makeStyles((theme) => ({
    root: {
        display: "flex",
    },
    card: {
        padding: "3rem",
        margin: "3rem"
    },
    centerText: {
        textAlign: "center"
    },
    feetablebutton: {
        marginRight: theme.spacing(1),
    },
    tableInputs: {
        margin: theme.spacing(1),
    },
    contentRoot: {
        flexGrow: 1,
        backgroundColor: theme.palette.background.paper,
        margin: "0px",
    },

    appBar: {
        transition: theme.transitions.create(["margin", "width"], {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
    },
    appBarShift: {
        width: `calc(100% - ${drawerWidth}px)`,
        marginLeft: drawerWidth,
        transition: theme.transitions.create(["margin", "width"], {
            easing: theme.transitions.easing.easeOut,
            duration: theme.transitions.duration.enteringScreen,
        }),
    },
    tablebutton: {
        margin: theme.spacing(1),
    },
    cell: {
        border: "1px solid black;",
    },
    menuButton: {
        marginRight: theme.spacing(2),
    },
    helpMenuButton: {
        marginLeft: theme.spacing(5),
        alignSelf: "right",
        textAlign: "right",
        marginRight: "2rem"
    },
    hide: {
        display: "none",
    },
    drawer: {
        width: drawerWidth,
        flexShrink: 0,
    },
    drawerPaper: {
        width: drawerWidth,
    },
    drawerHeader: {
        display: "flex",
        alignItems: "center",
        padding: theme.spacing(0, 1),
        // necessary for content to be below app bar
        ...theme.mixins.toolbar,
        justifyContent: "flex-end",
    },
    helpDrawer: (helpOpen: boolean) => {
        return {
            width: helpOpen ? drawerWidth + 300 : 0,
            flexShrink: 0,
        }
    },
    helpDrawerPaper: {
        width: drawerWidth + 300,

    },
    helpDrawerHeader: {
        display: "flex",
        alignItems: "center",
        padding: theme.spacing(0, 1),
        // necessary for content to be below app bar
        ...theme.mixins.toolbar,
        justifyContent: "flex-end",
    },

    content: {
        flexGrow: 1,
        padding: theme.spacing(3),
        transition: theme.transitions.create("margin", {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
        marginLeft: -drawerWidth,
        // overflowY: theme.overflowY,
    },
    contentShift: {
        transition: theme.transitions.create("margin", {
            easing: theme.transitions.easing.easeOut,
            duration: theme.transitions.duration.enteringScreen,
        }),
        marginLeft: 0,
    },
}));
