import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { PalavyrAlert } from "@common/components/PalavyrAlert";
import { getAnchorOrigin } from "@common/components/PalavyrSnackbar";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { Divider, makeStyles, Paper, Typography } from "@material-ui/core";
import { Alert, AlertTitle } from "@material-ui/lab";
import { SetState } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { Align } from "dashboard/layouts/positioning/Align";
import { SpaceEvenly } from "dashboard/layouts/positioning/SpaceEvenly";
import { DropzoneArea } from "material-ui-dropzone";
import * as React from "react";
import { useCallback, useContext, useEffect, useState } from "react";
import { SettingsWrapper } from "../SettingsWrapper";

const useStyles = makeStyles((theme) => ({
    paper: {
        backgroundColor: theme.palette.secondary.light,
        padding: "2rem",
        margin: "1rem",
        width: "100%",
        display: "inline-block",
        justifyContent: "center",
        textAlign: "center",
        position: "relative",
    },
    dropzone: {
        textAlign: "left",
        width: "100%",
    },
    logoPreview: {
        verticalAlign: "middle",
        padding: "0.5rem",
        maxWidth: "450px",
        maxHeight: "450px",
        textAlign: "center",
        marginBottom: "1.2rem"

    },
    previewChip: {
        minWidth: 130,
        maxWidth: 500,
        width: "auto",
        backgroundColor: "navy",
        color: "white",
        border: "black",
        overflow: "visible",
        textAlign: "left",
        display: "flex",
        justifyContent: "space-between",
    },
    deleteIcon: {
        color: "white",
    },
    label: {
        color: "white",
    },
    img: {
        maxWidth: "100%",
        maxHeight: "100%",
    },
    paperRoot: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
    },
    snackbarProps: {
        color: theme.palette.common.black,
    },
}));

export const ChangeLogoImage = () => {
    const [fileUpload, setFileUpload] = useState<File[]>([]);
    return <ChangeLogoImageInner fileUpload={fileUpload} setFileUpload={setFileUpload} />;
};

interface ChangeLogoImageInner {
    fileUpload: File[];
    setFileUpload: SetState<File[]>;
}
const ChangeLogoImageInner = ({ fileUpload, setFileUpload }: ChangeLogoImageInner) => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();
    const { setIsLoading } = useContext(DashboardContext);

    const [alertState, setAlertState] = useState<boolean>(false);
    const [companyLogo, setcompanyLogo] = useState<string>("");

    const alertMessage = {
        title: "Logo updated.",
        message: "",
    };

    const loadCompanyLogo = useCallback(async () => {
        const logoUri = await repository.Settings.Account.getCompanyLogo();
        setcompanyLogo(logoUri);
    }, []);

    const handleFileChange = (files: File[]) => {
        setFileUpload(files);
    };

    const handleFileDelete = (file: File) => {
        setFileUpload([file]);
    };

    const handleFileSave = async () => {
        setIsLoading(true);
        if (fileUpload !== null) {
            const formData = new FormData();
            formData.append("files", fileUpload[0]);
            const dataUrl = await repository.Settings.Account.updateCompanyLogo(formData);
            setcompanyLogo(dataUrl);
        }
        setFileUpload([]); // shouldn't this clear the chip
        setIsLoading(false);
    };

    const getDropRejectMessage = (rejectedFile: File, acceptedFiles: string[], maxFileSize: number) => {
        let message = `File ${rejectedFile.name} was rejected. `;
        if (!acceptedFiles.includes(rejectedFile.type)) {
            message += "File type not supported. ";
        }
        const maxFileSizeInGb = maxFileSize / 1000000 + " MB";
        if (rejectedFile.size > maxFileSize) {
            message += "File is too big. Size limit is " + maxFileSizeInGb;
        }

        return message;
    };

    const handleDeleteLogo = async () => {
        setIsLoading(true);
        await repository.Settings.Account.deleteCompanyLogo();
        setcompanyLogo("");
        setIsLoading(false);
    };

    useEffect(() => {
        loadCompanyLogo();
        return () => {
            setFileUpload([]);
        };
    }, []);

    const previewProps = { classes: { root: cls.previewChip, deleteIcon: cls.deleteIcon, label: cls.label } };

    const anchorOrigin = getAnchorOrigin("br");

    return (
        <SettingsWrapper>
            <AreaConfigurationHeader title="Change your company logo" subtitle="Update your company logo. This is used in the response email and pdf sent to customers." />
            <Divider />
            <Paper className={cls.paper}>
                <Alert style={{ marginBottom: "1.4rem" }} severity={companyLogo === "" ? "error" : "success"}>
                    <AlertTitle>
                        <Typography align="left" variant="h5">
                            {companyLogo === "" ? "Upload your company logo" : "Logo uploaded"}
                        </Typography>
                    </AlertTitle>
                    <Typography align="left" variant="body1" display="block">
                        Your company logo is placed into the top left area of each response PDF.
                    </Typography>
                    <Typography align="left" variant="body1" display="block">
                        For the best results, use a square 250px by 250px png or svg image.
                    </Typography>
                </Alert>
                {companyLogo !== "" && (
                    <Typography display="block" align="center" variant="h5" gutterBottom>
                        Your Current Logo
                    </Typography>
                )}
                <Align>
                    <div style={{ display: "flex", flexDirection: "column", justifyContent: "center", marginTop: "1.4rem" }}>
                        {companyLogo === "" ? (
                            "Upload a company logo"
                        ) : (
                            <Paper className={cls.logoPreview} classes={{ root: cls.paperRoot }}>
                                <img className={cls.img} src={companyLogo} />
                            </Paper>
                        )}
                    </div>
                </Align>
                <Divider />
                <div className={cls.dropzone}>
                    <DropzoneArea
                        showAlerts={true}
                        onChange={handleFileChange}
                        onDelete={handleFileDelete}
                        dropzoneText="Drag and drop a new image logo here or click"
                        useChipsForPreview
                        showPreviewsInDropzone={false}
                        previewChipProps={previewProps}
                        acceptedFiles={["image/png", "image/svg"]}
                        showPreviews={fileUpload.length > 0}
                        maxFileSize={500000}
                        previewGridProps={{ item: { alignContent: "flex-start" }, container: { spacing: 2, direction: "row" } }}
                        filesLimit={1}
                        previewText="Selected Files"
                        alertSnackbarProps={{ anchorOrigin, classes: { root: cls.snackbarProps } }}
                        getDropRejectMessage={getDropRejectMessage}
                    />
                    <PalavyrAlert alertState={alertState} setAlertState={setAlertState} useAlert alertMessage={alertMessage} />
                </div>
                <SpaceEvenly>
                    <SinglePurposeButton disabled={fileUpload.length === 0} variant="contained" color="primary" buttonText="Upload and Save" onClick={handleFileSave} />
                    <SinglePurposeButton disabled={companyLogo === ""} variant="contained" color="secondary" buttonText="Delete current logo" onClick={handleDeleteLogo} />
                </SpaceEvenly>
            </Paper>
        </SettingsWrapper>
    );
};
