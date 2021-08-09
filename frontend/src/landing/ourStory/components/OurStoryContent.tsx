import { Typography } from "@material-ui/core";
import React from "react";

export const TitleCopy = (props) => {
    return (
        <Typography variant="h6" paragraph>
            {props.children}
        </Typography>
    );
};

export const OurStoryContent = () => {
    return (
        <>
            <TitleCopy>Who we are</TitleCopy>
            <Typography paragraph>
                The word Palavyr (Palaver) means 'talk intended to charm or beguile'. Sure, there are a few different definitions for this word (some not so great), but this is the one we prefer. Why? Because
                we think that when you interact with someone, you should always be trying to get on their good side. And what better way to do that than to charm or beguile?
            </Typography>
            <Typography paragraph>
                Charming your customers can be a daunting task. Your customers have high expectations. To meet them, we have to repeat ourselves <b>a lot</b>. Not only that, but if you can't deliver to your
                customers the information they seek, then your business will likely suffer. And no one wants that.
            </Typography>
            <Typography paragraph>
                We at Palavyr understand this. We understand that making a great first impression on your potential customers is critical. We understand that helping your customers learn about your services is
                paramount. And we understand that the new generation of customers want self service.
            </Typography>
            <Typography paragraph>So who are we?</Typography>
            <Typography paragraph> We're the folks who created Palavyr. And that makes us your new best friends.</Typography>
        </>
    );
};
