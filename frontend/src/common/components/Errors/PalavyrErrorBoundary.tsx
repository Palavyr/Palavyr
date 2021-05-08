import { isDevelopmentStage } from "@api-client/clientUtils";
import { createStyles, makeStyles, Typography, WithStyles, withStyles } from "@material-ui/core";
import { Align } from "dashboard/layouts/positioning/Align";
import React, { ErrorInfo } from "react";
import { SinglePurposeButton } from "../SinglePurposeButton";

export interface PalavyrErrorBoundaryState {
    error: Error | null;
    errorInfo: ErrorInfo | null;
}

export interface PalavyrErrorBoundarProps extends WithStyles<typeof styles> {
    children: React.ReactNode;
}

const styles = (theme) =>
    createStyles({
        button: {
            marginBottom: "1rem",
        },
    });

export class PalavyrErrorBoundary extends React.Component<PalavyrErrorBoundarProps, PalavyrErrorBoundaryState> {
    constructor(props: PalavyrErrorBoundarProps) {
        super(props);
        this.state = { error: null, errorInfo: null };
    }

    componentDidCatch(error: Error, errorInfo: ErrorInfo) {
        // Catch errors in any components below and re-render with error message
        this.setState({
            error: error,
            errorInfo: errorInfo,
        });
        // You can also log error messages to an error reporting service here
    }

    render() {
        if (this.state.errorInfo) {
            return (
                <Align>
                    <div>
                        <Typography align="center" variant="h4">
                            Oh my gosh... Somethings gone wrong. We're terribly sorry!
                        </Typography>
                        <Typography align="center" variant="body1">
                            We realize this is a major inconvenience. This app is currently is beta testing and we are working hard to resolve these sorts of issues.
                        </Typography>
                        <Typography align="center" variant="body1">
                            If you can, please report this problem to us at info.palavyr@gmail.com
                        </Typography>
                        {isDevelopmentStage() && (
                            <details style={{ whiteSpace: "pre-wrap" }}>
                                {this.state.error && this.state.error.toString()}
                                <br />
                                {this.state.errorInfo.componentStack}
                            </details>
                        )}
                        <Align>
                            <SinglePurposeButton classes={this.props.classes.button} variant="outlined" color="primary" buttonText="Reload" onClick={() => window.location.reload()} />
                        </Align>
                    </div>
                </Align>
            );
        }
        return this.props.children;
    }
}

export default withStyles(styles, { withTheme: true })(PalavyrErrorBoundary);
