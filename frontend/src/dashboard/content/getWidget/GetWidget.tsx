import React, { useEffect, useCallback, useState } from "react"
import { Typography, Divider, Card, makeStyles } from "@material-ui/core"
import { CodeCard } from "./CodeCard"
import { ApiClient } from "@api-client/Client"
import { HelpTypes } from "dashboard/layouts/DashboardLayout"



const useStyles = makeStyles(theme => ({
    outerCard: {
        margin: "4rem",
        padding: "3rem",
        textAlign: "center"
    }
}))

interface IGetWidget {
    setHelpType(helpType: HelpTypes): void;
}
export const GetWidget = ({setHelpType}: IGetWidget) => {

    const client = new ApiClient();
    const [apikey, setApiKey] = useState<string>("");
    const classes = useStyles();
    setHelpType("enquiries");
    const loadApiKey = useCallback(async () => {
        var key = (await client.Settings.Account.getApiKey()).data as string;
        setApiKey(key);
    }, [])

    useEffect(() => {
        loadApiKey();
    }, [])


    return (
        <Card className={classes.outerCard}>
            <Typography gutterBottom={true} variant="h3">
                Add your configured widget to your website
            </Typography>
            {/* <br /> */}
            {/* <Divider variant="middle"/> */}
            <Typography paragraph>
                Adding the widget toy our website is so easy! Simply paste the following code into your website's html and apply custom styling:
            </Typography>

            <Typography component={"pre"} paragraph>
                <strong>&lt;iframe src="https://widget.palavyr.com/widget?key={apikey}" /&gt;</strong>
            </Typography>
        </Card>
    )

}