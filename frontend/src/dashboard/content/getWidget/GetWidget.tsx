import React, { useEffect, useCallback, useState } from "react";
import { Typography, Card, makeStyles } from "@material-ui/core";
import { ApiClient } from "@api-client/Client";

const useStyles = makeStyles((theme) => ({
    outerCard: {
        margin: "4rem",
        padding: "3rem",
        textAlign: "center",
    },
}));

export const GetWidget = () => {
    const client = new ApiClient();
    const [apikey, setApiKey] = useState<string>("");
    const classes = useStyles();

    const loadApiKey = useCallback(async () => {
        const { data: key } = await client.Settings.Account.getApiKey();
        console.log(`ApiKey: ${key}`);
        setApiKey(key);
    }, []);

    useEffect(() => {
        loadApiKey();
    }, []);

    return (
        <>
            <Card className={classes.outerCard}>
                <Typography gutterBottom={true} variant="h3">
                    Add your configured widget to your website
                </Typography>

                <Typography paragraph>Adding the widget toy our website is so easy! Simply paste the following code into your website's html and apply custom styling:</Typography>
                {apikey !== "" && (
                    <Typography component="pre" paragraph>
                        <strong>&lt;iframe src="https://widget.palavyr.com/widget?key={apikey}" /&gt;</strong>
                    </Typography>
                )}
            </Card>
        </>
    );
};
