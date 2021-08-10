import { ZoomImage } from "@common/components/borrowed/ZoomImage";
import { LineSpacer } from "@common/components/typography/LineSpacer";
import { TitleCopy } from "@common/components/typography/TitleCopy";
import { BlogCopy } from "@landing/blog/components/BlogCopy";
import { Typography } from "@material-ui/core";
import React from "react";
import { Link } from "react-router-dom";

import Step1 from "./Step1.gif";
import Step2Code from "./Step2Code.png";
import Step3Code2 from "./Step3Code-2.png";

export const Post2 = (
    <>
        <TitleCopy>Your first set of Palavyr conversations are done</TitleCopy>
        <BlogCopy>
            If this is the case for you, then let me say - <strong>Congratulations!</strong>
        </BlogCopy>
        <BlogCopy>
            Look, we said you <i>could</i> build your own chat bot, but we never said it wouldn't be without its challenges. Coming up with the perfect tone and just the right number of interrupting questions
            to make sure your customers are still paying attention to your widget takes time, and we applaud you for your efforts. But now that you've got a shining conversation designed and your widget is
            ready for action, what do you do next?
        </BlogCopy>
        <br></br>
        <TitleCopy>Moving your widget into the website</TitleCopy>
        <BlogCopy>
            Websites are complicated things. They are generally written using HTML - a type of code used for creating webpages. This code can be somewhat difficult to understand, so we usually hire experts to
            take care of it for us.
            <i> And if you are already either bored, confused, or just plain not interested, then know that Palavyr was built for you.</i> Unfortunately, to place the widget in your website, ultimately we're
            going to need to modify that code. If you're using a website builder, you win a get-out-of-jail-free card on this one. We'll show you how to place your widget in a site like{" "}
            <a href="https://www.wix.com">Wix.com</a> in a future post.
        </BlogCopy>
        <TitleCopy>Step 1 - locate your widget script and copy it</TitleCopy>
        <BlogCopy>
            You technically only need to copy a single line of code in order to get the widget into your website. You can find this code by clicking on the Get Widget link in the side bard navigation after
            you've logged into the configuration dashboard.
        </BlogCopy>
        <ZoomImage alt="step 1" imgSrc={Step1} />
        <br></br>
        <br></br>

        <TitleCopy>Step 2 - insert the widget code into your website</TitleCopy>

        <BlogCopy>
            Depending on how you have built your website, there are a few different ways you can insert your Palavyr widget into your website. This post is going to cover the most basic of cases - a basic HTML
            webpage.
        </BlogCopy>
        <BlogCopy>
            The easiest way to think about the Palavyr widget is by understanding what the widget really is under the hood - a simple webpage. You can see what I mean by clicking this link:{" "}
            <Link to="https://widget.palavyr.com/widget?key=b9b9fb77-8428-4e2c-8555-6c4c370c20d8">https://widget.palavyr.com/widget?key=b9b9fb77-8428-4e2c-8555-6c4c370c20d8</Link>. This is the url where the
            palavyr demo widget lives. When you visit that website, you'll see that the chatbot pops up and that it takes up the entire webpage.
            <br></br>
            <br></br> You'll also notice that we've placed the api key in the address as
            <br></br>
            <br></br>
            <Typography component="pre">?key=b9b9fb77-8428-4e2c-8555-6c4c370c20d8</Typography>
        </BlogCopy>
        <BlogCopy>
            When you put the widget in your webpage, what you will actually do is <i>embed</i> the widget webpage inside of your webpage using something called an <i>iFrame</i>. It sounds like an Apple product,
            but it's actually a way to create mini web pages inside of other webpages. So to do this, you paste the iFrame code from the <b>Get Widget</b> page into your HTML code.
        </BlogCopy>
        <ZoomImage alt="step 2 code" imgSrc={Step2Code} />
        <br></br>
        <br></br>
        <TitleCopy>Step 3. Style your widget</TitleCopy>
        <BlogCopy>
            The final step for your widget is to apply styles. You are free to apply whatever styles and scripting you would like in order to customize the placement of the widget in your website. We suggest
            placing it somewhere that has high visibility and provides an animation that will draw attention. You can also choose the size of the widget, how it appears and disappears, and more. If you have a
            webmaster that handles the content and styling of your website, be sure to reach out to them.
        </BlogCopy>
        <BlogCopy>
            One thing to be aware of is that if you don't provide some 'minimal' styling to the widget right, it won't render correctly. A single line of HTML code seems pretty easy to manage, but we can
            actually do even better. In the near future, we'll be producing a script tag for your HTML that will allow you to simply load a pre-styled widget (you will of course be able to style it yourself as
            well if you wish).
        </BlogCopy>
        <BlogCopy>In the meantime, we've provided some basic styles you can start with below:</BlogCopy>
        <ZoomImage alt="step 3 code" imgSrc={Step3Code2} />

        <LineSpacer numLines={4} />
        <TitleCopy>...And thats it!</TitleCopy>
        <BlogCopy>With this, you'll have your widget placed and customers happily engaging in no time!</BlogCopy>
    </>
);
