import { makeStyles } from "@material-ui/core";

type CardStyle = {
    border?: boolean;
};

export const useStyles = makeStyles((theme) => ({
    icon: {
        marginTop: "2rem",
        marginBottom: "1rem",
        fontSize: "45pt",
    },
    title: {
        paddingTop: "1rem",
        paddingBottom: "1rem",
    },
    price: {
        paddingTop: "3.2rem",
        paddingBottom: "1.2rem",
        display: "inline-block",
    },
    tablecontainer: {
        marginRight: "10%",
        marginLeft: "10%",
        marginBottom: "2rem",
        textAlign: "center",
    },
    table: {
        padding: "0.3rem",
        marginTop: "4rem",
        marginBottom: "6rem",
    },
    tableBody: {
        padding: "1rem",
    },
    tableRow: {
        borderBottom: "2px solid lightgray",
        margin: "0px",
    },
    tableRoot: {
        paddingTop: "0.8rem",
        paddingBottom: "0.8rem",
    },
    tablecellLeft: {
        padding: "0px",
        fontSize: "16px",
        color: "white",
    },
    tablecellRight: {
        textAlign: "center",
        fontSize: "16px",
        color: "white",
    },
    yes: {
        color: "#01C448",
    },
    no: {
        color: "#E11010",
    },
    money: {
        fontFamily: "'Noto Sans TC', sans-serif",
    },

    // background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)"
}));

export const pricingContainerStyles = makeStyles((theme) => ({
    body: {
        display: "flex",
        justifyContent: "space-evenly",
        background: "radial-gradient(circle, rgba(238,241,244,1) 28%, rgba(211,224,227,1) 76%)",
        paddingRight: "14%",
        paddingLeft: "14%",
        borderRadius: "4px",
    },
    paperCommon: {
        margin: "1rem",
        width: "28%",
        textAlign: "center",
        borderRadius: "5px",
        alignContent: "center",
        marginTop: "3rem",
    },
    paperFree: (props: CardStyle) => {
        return {
            backgroundColor: "#0093CB",
            color: "white",
            border: props.border ? "3px solid black" : "0px solid black",
        };
    },
    paperPremium: (props: CardStyle) => {
        return {
            backgroundColor: "#014B91",
            color: "white",
            border: props.border ? "3px solid black" : "0px solid black",
        };
    },
    paperPro: (props: CardStyle) => {
        return {
            backgroundColor: "#011E6D",
            color: "white",
            border: props.border ? "3px solid black" : "0px solid black",
        };
    },
}));
