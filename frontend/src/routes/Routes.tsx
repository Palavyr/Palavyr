import React, { useState } from "react";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import { LandingPage } from "@landing/Landing";
import { ProtectedRoute } from "@protected-routes";
import { DashboardLayout } from "dashboard/layouts/DashboardLayout";
import { Success } from "dashboard/content/purchse/success/Success";
import { Cancel } from "dashboard/content/purchse/cancel/Cancel";
import { Subscribe } from "dashboard/content/subscribe/Subscribe";
import { HelpTypes } from "@Palavyr-Types";
import { SubscribeHelp } from "dashboard/content/help/SubscribeHelp";
import { AreaContent } from "dashboard/content/responseConfiguration/AreaContent";
import { EditorHelp } from "dashboard/content/help/EditorHelp";
import { Enquires } from "dashboard/content/enquiries/Enquiries";
import { GetWidget } from "dashboard/content/getWidget/GetWidget";
import { WelcomeToTheDashboard } from "dashboard/content/welcome/WelcomeToTheDashboard";
import { ConversationHelp } from "dashboard/content/help/ConversationHelp";
import { EmailHelp } from "dashboard/content/help/EmailHelp";
import { EstimateHelp } from "dashboard/content/help/EstimateHelp";
import { AttachmentsHelp } from "dashboard/content/help/AttachmentsHelp";
import { AreaSettingsHelp } from "dashboard/content/help/AreaSettingsHelp";
import { PreviewHelp } from "dashboard/content/help/PreviewHelp";
import { PleaseConfirmYourEmail } from "dashboard/content/welcome/PleaseConfirmYourEmail";
import { Purchase } from "dashboard/content/purchse/Purchase";
import { ChatDemoHelp } from "dashboard/content/help/ChatDemoHelp";
import { SettingsContent } from "dashboard/content/settings/SettingsContent";
import { ChatDemo } from "dashboard/content/demo/ChatDemo";
import { GetWidgetHelp } from "dashboard/content/help/GetWidgetHelp";
import { EnquiriesHelp } from "dashboard/content/help/EnquiriesHelp";
import { PurchaseHelp } from "dashboard/content/help/PurchaseHelp";
import { SuccessHelp } from "dashboard/content/help/SuccessHelp";
import { WelcomeToTheDashboardHelp } from "dashboard/content/help/WelcomeToTheDashboardHelp";
import { CancelHelp } from "dashboard/content/help/CancelHelp";
import { PleaseConfirmYourEmailHelp } from "dashboard/content/help/PleaseConfirmYourEmailHelp";
import { AuthContext, DashboardContext } from "dashboard/layouts/DashboardContext";
import Auth from "auth/Auth";

function withLayout<P>(ContentComponent: React.ComponentType<P>, helpComponent: React.ReactNode) {


    const ComponentWithHelp = (props: P) => {
        return (
            <AuthContext.Provider value={{ isActive: Auth.accountIsActive, isAuthenticated: Auth.accountIsAuthenticated}}>
                <DashboardLayout helpComponent={helpComponent}>
                    <ContentComponent {...props} />
                </DashboardLayout>
            </AuthContext.Provider>
        );
    };
    return ComponentWithHelp;
}


export const Routes = () => {
    return (
        <Router>
            <Route exact path="/" component={LandingPage} />
            <ProtectedRoute exact path="/dashboard/" component={withLayout(WelcomeToTheDashboard, WelcomeToTheDashboardHelp)} />
            <ProtectedRoute exact path="/dashboard/welcome" component={withLayout(WelcomeToTheDashboard, WelcomeToTheDashboardHelp)} />

            <ProtectedRoute exact path="/dashboard/editor/:areaIdentifier" component={withLayout(AreaContent, EditorHelp)} />

            <ProtectedRoute exact path="/dashboard/settings/" component={withLayout(SettingsContent, SubscribeHelp)} />
            <ProtectedRoute exact path="/dashboard/demo/" component={withLayout(ChatDemo, ChatDemoHelp)} />
            <ProtectedRoute exact path="/dashboard/getWidget/" component={withLayout(GetWidget, GetWidgetHelp)} />
            <ProtectedRoute exact path="/dashboard/enquiries/" component={withLayout(Enquires, EnquiriesHelp)} />

            <ProtectedRoute exact path="/dashboard/subscribe" component={withLayout(Subscribe, SubscribeHelp)} />
            <ProtectedRoute exact path="/dashboard/subscribe/purchase/" component={withLayout(Purchase, PurchaseHelp)} />
            <ProtectedRoute exact path="/dashboard/subscribe/success" component={withLayout(Success, SuccessHelp)} />
            <ProtectedRoute exact path="/dashboard/subscribe/cancel" component={withLayout(Cancel, CancelHelp)} />

            <ProtectedRoute exact path="/dashboard/confirm" component={withLayout(PleaseConfirmYourEmail, PleaseConfirmYourEmailHelp)} />
        </Router>
    );
};

{
    /* {loaded === true && (active === false) && <PleaseConfirmYourEmail />}
                {contentType === "editor" && (active === true) && <AreaContent checkAreaCount={checkAreaCount} setHelpType={setHelpType} active={active} areaIdentifier={areaIdentifier} areaName={currentViewName} setLoaded={setLoaded} setViewName={setViewName} />}
                {contentType === undefined && active === true && <WelcomeToTheDashboard     ={checkAreaCount} />}

                {/* {helpType === "conversation" && <ConversationHelp defaultOpen />}
                {helpType === "editor" && <EditorHelp defaultOpen />}
                {helpType === "email" && <EmailHelp defaultOpen />}

                {helpType === "estimate" && <EstimateHelp defaultOpen />}
                {helpType === "attachments" && <AttachmentsHelp defaultOpen />}
                {helpType === "areasettings" && <AreaSettingsHelp defaultOpen />}
                {helpType === "preview" && <PreviewHelp defaultOpen />} */
}

{
    /* {(helpType === "settings") && <SettingsHelp />}
                {(helpType === "demo") && <DemoHelp />}
                {(helpType === "enquiries") && <EnquiryHelp />}
                {(helpType === "getwidget") && <GetWidgetHelp />}
                {(helpType === "subscrible") && <SubscribeHelp />}
                {(helpType === "password") && <PasswordHelp />}
                {(helpType === "companyname") && <CompanyNameHelp />}
                {(helpType === "phonenumber") && <PhoneNumberHelp />}
                {(helpType === "logo") && <LogoHelp />}
                {(helpType === "locale") && <LocaleHelp />} */
}
