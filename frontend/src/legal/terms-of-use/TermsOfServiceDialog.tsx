import React from "react";

import ArrowBackIcon from "@material-ui/icons/ArrowBack";
import { makeStyles, useTheme, Dialog, DialogTitle, DialogContent, Typography, DialogActions, Button } from "@material-ui/core";

export interface ITermsOfServiceDialog {
    onClose: any;
}

const useStyles = makeStyles((theme) => ({
    termsConditionsListitem: {
        marginLeft: theme.spacing(3),
        marginTop: theme.spacing(1),
    },
    dialogActions: {
        justifyContent: "flex-start",
        paddingTop: theme.spacing(2),
        paddingBottom: theme.spacing(2),
        paddingRight: theme.spacing(2),
    },

    backButton: {},
}));

export const TermsOfServiceDialog = ({ onClose }: ITermsOfServiceDialog) => {
    const classes = useStyles();
    const theme = useTheme();

    return (
        <Dialog open scroll="paper" onClose={onClose} hideBackdrop>
            <DialogTitle>Terms and Conditions</DialogTitle>
            <DialogContent>
                <h2>
                    <strong>Terms and Conditions</strong>
                </h2>

                <p>Welcome to Palavyr.com!</p>

                <p>These terms and conditions outline the rules and regulations for the use of Palavyr.com's Website, located at https://www.palavyr.com.</p>

                <p>
                    By accessing this website we assume you accept these terms and conditions. Do not continue to use Palavyr.com if you do not agree to take all of the terms and conditions stated on this page.
                </p>

                <p>
                    The following terminology applies to these Terms and Conditions, Privacy Statement and Disclaimer Notice and all Agreements: "Client", "You" and "Your" refers to you, the person log on this
                    website and compliant to the Company’s terms and conditions. "The Company", "Ourselves", "We", "Our" and "Us", refers to our Company. "Party", "Parties", or "Us", refers to both the Client
                    and ourselves. All terms refer to the offer, acceptance and consideration of payment necessary to undertake the process of our assistance to the Client in the most appropriate manner for the
                    express purpose of meeting the Client’s needs in respect of provision of the Company’s stated services, in accordance with and subject to, prevailing law of Netherlands. Any use of the above
                    terminology or other words in the singular, plural, capitalization and/or he/she or they, are taken as interchangeable and therefore as referring to same.
                </p>

                <h3>
                    <strong>Cookies</strong>
                </h3>

                <p>We employ the use of cookies. By accessing Palavyr.com, you agreed to use cookies in agreement with the Palavyr.com's Privacy Policy. </p>

                <p>
                    Most interactive websites use cookies to let us retrieve the user’s details for each visit. Cookies are used by our website to enable the functionality of certain areas to make it easier for
                    people visiting our website. Some of our affiliate/advertising partners may also use cookies.
                </p>

                <h3>
                    <strong>License</strong>
                </h3>

                <p>
                    Unless otherwise stated, Palavyr.com and/or its licensors own the intellectual property rights for all material on Palavyr.com. All intellectual property rights are reserved. You may access
                    this from Palavyr.com for your own personal use subjected to restrictions set in these terms and conditions.
                </p>

                <p>You must not:</p>
                <ul>
                    <li>Republish material from Palavyr.com</li>
                    <li>Sell, rent or sub-license material from Palavyr.com</li>
                    <li>Reproduce, duplicate or copy material from Palavyr.com</li>
                    <li>Redistribute content from Palavyr.com</li>
                </ul>

                <p>This Agreement shall begin on the date hereof.</p>

                <p>
                    Parts of this website offer an opportunity for users to post and exchange opinions and information in certain areas of the website. Palavyr.com does not filter, edit, publish or review
                    Comments prior to their presence on the website. Comments do not reflect the views and opinions of Palavyr.com,its agents and/or affiliates. Comments reflect the views and opinions of the
                    person who post their views and opinions. To the extent permitted by applicable laws, Palavyr.com shall not be liable for the Comments or for any liability, damages or expenses caused and/or
                    suffered as a result of any use of and/or posting of and/or appearance of the Comments on this website.
                </p>

                <p>Palavyr.com reserves the right to monitor all Comments and to remove any Comments which can be considered inappropriate, offensive or causes breach of these Terms and Conditions.</p>

                <p>You warrant and represent that:</p>

                <ul>
                    <li>You are entitled to post the Comments on our website and have all necessary licenses and consents to do so;</li>
                    <li>The Comments do not invade any intellectual property right, including without limitation copyright, patent or trademark of any third party;</li>
                    <li>The Comments do not contain any defamatory, libelous, offensive, indecent or otherwise unlawful material which is an invasion of privacy</li>
                    <li>The Comments will not be used to solicit or promote business or custom or present commercial activities or unlawful activity.</li>
                </ul>

                <p>
                    You hereby grant Palavyr.com a non-exclusive license to use, reproduce, edit and authorize others to use, reproduce and edit any of your Comments or content in any and all forms, formats or
                    media.
                </p>

                <h3>
                    <strong>Hyperlinking to our Content</strong>
                </h3>

                <p>The following organizations may link to our Website without prior written approval:</p>

                <ul>
                    <li>Government agencies;</li>
                    <li>Search engines;</li>
                    <li>News organizations;</li>
                    <li>Online directory distributors may link to our Website in the same manner as they hyperlink to the Websites of other listed businesses; and</li>
                    <li>System wide Accredited Businesses except soliciting non-profit organizations, charity shopping malls, and charity fundraising groups which may not hyperlink to our Web site.</li>
                </ul>

                <p>
                    These organizations may link to our home page, to publications or to other Website information so long as the link: (a) is not in any way deceptive; (b) does not falsely imply sponsorship,
                    endorsement or approval of the linking party and its products and/or services; and (c) fits within the context of the linking party’s site.
                </p>

                <p>We may consider and approve other link requests from the following types of organizations:</p>

                <ul>
                    <li>commonly-known consumer and/or business information sources;</li>
                    <li>dot.com community sites;</li>
                    <li>associations or other groups representing charities;</li>
                    <li>online directory distributors;</li>
                    <li>internet portals;</li>
                    <li>accounting, law and consulting firms; and</li>
                    <li>educational institutions and trade associations.</li>
                </ul>

                <p>
                    We will approve link requests from these organizations if we decide that: (a) the link would not make us look unfavorably to ourselves or to our accredited businesses; (b) the organization
                    does not have any negative records with us; (c) the benefit to us from the visibility of the hyperlink compensates the absence of Palavyr.com; and (d) the link is in the context of general
                    resource information.
                </p>

                <p>
                    These organizations may link to our home page so long as the link: (a) is not in any way deceptive; (b) does not falsely imply sponsorship, endorsement or approval of the linking party and
                    its products or services; and (c) fits within the context of the linking party’s site.
                </p>

                <p>
                    If you are one of the organizations listed in paragraph 2 above and are interested in linking to our website, you must inform us by sending an e-mail to Palavyr.com. Please include your
                    name, your organization name, contact information as well as the URL of your site, a list of any URLs from which you intend to link to our Website, and a list of the URLs on our site to
                    which you would like to link. Wait 2-3 weeks for a response.
                </p>

                <p>Approved organizations may hyperlink to our Website as follows:</p>

                <ul>
                    <li>By use of our corporate name; or</li>
                    <li>By use of the uniform resource locator being linked to; or</li>
                    <li>By use of any other description of our Website being linked to that makes sense within the context and format of content on the linking party’s site.</li>
                </ul>

                <p>No use of Palavyr.com's logo or other artwork will be allowed for linking absent a trademark license agreement.</p>

                <h3>
                    <strong>iFrames</strong>
                </h3>

                <p>Without prior approval and written permission, you may not create frames around our Webpages that alter in any way the visual presentation or appearance of our Website.</p>

                <h3>
                    <strong>Content Liability</strong>
                </h3>

                <p>
                    We shall not be held responsible for any content that appears on your Website. You agree to protect and defend us against all claims that is rising on your Website. No link(s) should appear
                    on any Website that may be interpreted as libelous, obscene or criminal, or which infringes, otherwise violates, or advocates the infringement or other violation of, any third party rights.
                </p>

                <h3>Indemnification</h3>
                <p>
                    Palavyr.com, its owners, and its operators shall not be held responsible for any content that is interpreted as racist, demeaning, sexist, or otherwise demeaning to any individual or group.
                    All content uploaded to Palavyr.com reflects the views and opinions soley of the user. Palavyr.com shall be held in no way responsible for anything that results from the use of the Palavyr
                    service.
                </p>
                <p>Palavyr.com, its ownders, and its operators shall not be held responsible for any damages of any kind the result from using the Palavyr service.</p>

                <h3>
                    <strong>Your Privacy</strong>
                </h3>

                <p>Please read Privacy Policy</p>

                <h3>Financial</h3>
                <p>Palavyr.com uses Stripe.com to process payments. Palavyr does not retain any financial information about individuals or business that use the Palavyr.com service.</p>
                <h3>
                    <strong>Reservation of Rights</strong>
                </h3>

                <p>
                    We reserve the right to request that you remove all links or any particular link to our Website. You approve to immediately remove all links to our Website upon request. We also reserve the
                    right to amen these terms and conditions and it’s linking policy at any time. By continuously linking to our Website, you agree to be bound to and follow these linking terms and conditions.
                </p>

                <h3>
                    <strong>Removal of links from our website</strong>
                </h3>

                <p>
                    If you find any link on our Website that is offensive for any reason, you are free to contact and inform us any moment. We will consider requests to remove links but we are not obligated to
                    or so or to respond to you directly.
                </p>

                <p>
                    We do not ensure that the information on this website is correct, we do not warrant its completeness or accuracy; nor do we promise to ensure that the website remains available or that the
                    material on the website is kept up to date.
                </p>

                <h3>
                    <strong>Disclaimer</strong>
                </h3>

                <p>
                    To the maximum extent permitted by applicable law, we exclude all representations, warranties and conditions relating to our website and the use of this website. Nothing in this disclaimer
                    will:
                </p>

                <ul>
                    <li>limit or exclude our or your liability for death or personal injury;</li>
                    <li>limit or exclude our or your liability for fraud or fraudulent misrepresentation;</li>
                    <li>limit any of our or your liabilities in any way that is not permitted under applicable law; or</li>
                    <li>exclude any of our or your liabilities that may not be excluded under applicable law.</li>
                </ul>

                <p>
                    The limitations and prohibitions of liability set in this Section and elsewhere in this disclaimer: (a) are subject to the preceding paragraph; and (b) govern all liabilities arising under
                    the disclaimer, including liabilities arising in contract, in tort and for breach of statutory duty.
                </p>

                <p>As long as the website and the information and services on the website are provided free of charge, we will not be liable for any loss or damage of any nature.</p>

                {/* <Typography variant="h6" color="primary" paragraph>
                    Introduction
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua. At vero eos et accusam et justo duo dolores et ea
                    rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem
                    ipsum dolor sit amet.
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua. At vero eos et accusam et justo duo dolores et ea
                    rebum. Stet clita kasd gubergren.
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Intellectual Property Rights
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua. At vero eos et accusam et justo duo dolores et ea
                    rebum. Stet clita kasd gubergren,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Restrictions
        </Typography>
                <Typography paragraph>
                    You are specifically restricted from all of the following:
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua.
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography className={classes.termsConditionsListitem}>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography className={classes.termsConditionsListitem} paragraph>
                    - Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua. At vero eos et accusam et justo duo dolores et ea
                    rebum. Stet clita kasd gubergren,
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Your Content
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    No warranties
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua.
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Limitation of liability
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Indemnification
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Severability
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
                    sed diam voluptua.
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Variation of Terms
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Assignment
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Entire Agreement
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography>
                <Typography variant="h6" color="primary" paragraph>
                    Governing Law & Jurisdiction
        </Typography>
                <Typography paragraph>
                    Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
                    nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
        </Typography> */}
                <Typography>
                    A digital copy of these terms can be found{" "}
                    <a target="_blank" href="https://www.termsandconditionsgenerator.com/live.php?token=vLs9X3tLNJpcdLzUDFtEbGXnXmo41VxE">
                        here
                    </a>
                </Typography>
            </DialogContent>
            <DialogActions className={classes.dialogActions}>
                {/* <ColoredButton classes={classes.backButton} onClick={onClose} variant="contained" color="secondary"> */}
                <Button onClick={onClose} variant="outlined">
                    <ArrowBackIcon onClick={onClose} />
                    Back
                </Button>
                {/* </ColoredButton> */}
            </DialogActions>
        </Dialog>
    );
};