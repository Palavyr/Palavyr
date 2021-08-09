import { TitleCopy } from "@landing/ourStory/components/OurStoryContent";
import { Typography } from "@material-ui/core";
import React from "react";

const BlogCopy = (props) => {
    return (
        <Typography variant="body2" gutterBottom style={{ paddingBottom: "1rem" }}>
            {props.children}
        </Typography>
    );
};
export const Post1 = (
    <>
        <TitleCopy variant="h6" paragraph>
            Palavyr has officially launched!
        </TitleCopy>
        <BlogCopy paragraph>
            Its been a long road, but we have finally reached a minimal viable product for Palavyr! Even so, this product is rich with incredivle features you can lean on to create an incredible first
            impressions for your customers! Lets take a look at a few features that stand out.
        </BlogCopy>

        <br></br>
        <TitleCopy>Brand Customization</TitleCopy>
        <BlogCopy paragraph>
            One of the most important components to standing out online is your branding. When people see your branding they should immediately associate it with your business. the visual aestetic of your
            business online is your brand colors. If they don't, you should reconsider your branding strategy! Thats because brand recognition is what will help you spread your business to new customers and
            propogate awareness. Its all about branding!
        </BlogCopy>
        <BlogCopy paragraph>
            Branding isn't just about your logo either. Its about your fonts, your color palette, your taglines, and so much more. Palavyr lets you stay on brand by offering various types of customization.
            Using the chat demo customization tool, you can rebrand your Palavyr chat widget to use your taglines and colors. And, of course, when you write your conversations, you can infuse them with your
            brand's personality!
        </BlogCopy>
        <BlogCopy>Brand customization isn't the only feature you're going to love!</BlogCopy>

        <br></br>
        <TitleCopy>Fully customizable conversations</TitleCopy>
        <BlogCopy>
            This list would be utterly incomplete if we didn't mention the Palavyr Conversation editor! This is the heart and soul of Palavyr. One of the guiding principles behind Palavyr is that you, as the
            user of Palavyr and the person that wants a chat bot for their website, should have the ability write your chat conversations. If you aren't in control of writting the chat text, but do decide what
            it says, then you likely have to rely on a programmer. Worse than that, you have to rely on the programmer to create the initial conversation, as well as to make any major or minor modifications!
        </BlogCopy>
        <BlogCopy>With Palavyr's conversation editor, you can log on and make an update to your chat any time you like. If you ask us, thats pretty powerful.</BlogCopy>

        <br></br>
        <TitleCopy>Enquiry Reviews</TitleCopy>
        <BlogCopy>
            We think you deserve to know when someone engages with your chat bot. Thats why we've included an enquiry review page where you can visually inspect all of the conversations your users have had with
            your chat bot. We also think you deserve to learn something about your customers when they use your chat bot. You'll get all kinds of great insight into your customers enquiries, and if you
            stimulate users with a few cleverly designed questions, you can even learn a great deal about them as well.
        </BlogCopy>

        <br></br>
        <TitleCopy>Configurable Email Responses</TitleCopy>
        <BlogCopy>There are few experiences that are more frustrating than interacting with a chatbot that does. basically. nothing. I mean what is the point?  </BlogCopy>
    </>
);
