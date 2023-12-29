import { HeaderStrip } from "@common/components/HeaderStrip";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { Divider, makeStyles, Paper } from "@material-ui/core";
import { Alert, AlertTitle } from "@material-ui/lab";
import { FileAssetResource } from "@common/types/api/EntityResources";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { Align } from "@common/positioning/Align";
import { SpaceEvenly } from "@common/positioning/SpaceEvenly";
import * as React from "react";
import { useCallback, useContext, useEffect, useState } from "react";
import { SettingsWrapper } from "../SettingsWrapper";
import { UploadOrSelectFromExisting } from "@common/uploads/UploadOrChooseFromExisting";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { ZoomImage } from "@common/components/borrowed/ZoomImage";


const useStyles = makeStyles<{}>((theme: any) => ({
    paper: {
        backgroundColor: "rgb(0, 0, 0 ,0)", //theme.palette.secondary.light,
        border: "0px",
        boxShadow: "none",
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
        marginBottom: "1.2rem",
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
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();

    const [companyLogo, setcompanyLogo] = useState<FileAssetResource>({ id: 0, fileId: "", fileName: "", link: "" });

    const loadCompanyLogo = useCallback(async () => {
        const logoFileAssetResource = await repository.Settings.Account.GetCompanyLogo();
        setcompanyLogo(logoFileAssetResource);
    }, []);

    const handleFileChange = async (_: any, fileAssetResource: FileAssetResource) => {
        await repository.Configuration.FileAssets.LinkFileAssetToLogo(fileAssetResource.fileId);
        setcompanyLogo(fileAssetResource);
    };

    const handleFileSave = async (files: File[]) => {
        const formData = new FormData();
        formData.append("files", files[0]);

        const fileAssetResources = await repository.Configuration.FileAssets.UploadFileAssets(formData);
        const fileAssetResource = await repository.Configuration.FileAssets.LinkFileAssetToLogo(fileAssetResources[0].fileId);
        setcompanyLogo(fileAssetResource);
    };

    const handleDeleteLogo = async () => {
        await repository.Settings.Account.DeleteCompanyLogo();
        setcompanyLogo({ id: 0, fileId: "", fileName: "", link: "" });
    };

    useEffect(() => {
        loadCompanyLogo();
        return () => {
            setcompanyLogo({ id: 0, fileId: "", fileName: "", link: "" });
        };
    }, []);

    return (
        <SettingsWrapper>
            <HeaderStrip title="Select your Logo" subtitle="Update a logo that will be used in the response email and pdf sent to customers." />
            <Paper className={cls.paper}>
                <Alert style={{ marginBottom: "1.4rem" }} severity={!companyLogo?.fileId ? "error" : "success"}>
                    <AlertTitle>
                        <PalavyrText align="left" variant="h5">
                            {!companyLogo?.fileId ? "Provide your company logo" : "Logo uploaded"}
                        </PalavyrText>
                    </AlertTitle>
                    <PalavyrText align="left" variant="body1" display="block">
                        Your company logo is placed into the top left intentof each response PDF.
                    </PalavyrText>
                    <PalavyrText align="left" variant="body1" display="block">
                        For the best results, use a square 250px by 250px png or svg image.
                    </PalavyrText>
                </Alert>
                {companyLogo?.fileId && (
                    <PalavyrText display="block" align="center" variant="h5" gutterBottom>
                        Your Current Logo
                    </PalavyrText>
                )}
                <Align>
                    <div style={{ display: "flex", flexDirection: "column", justifyContent: "center", marginTop: "1.4rem" }}>
                        {!companyLogo?.fileId ? (
                            "Upload a company logo"
                        ) : (
                            <Paper className={cls.logoPreview} classes={{ root: cls.paperRoot }}>
                                <ZoomImage alt="logo" imgSrc={companyLogo?.link} className={cls.img} />
                            </Paper>
                        )}
                    </div>
                </Align>
                <Divider />
                <UploadOrSelectFromExisting currentFileAssetId={companyLogo?.fileId} handleFileSave={handleFileSave} onSelectChange={handleFileChange} summary={"Upload"} uploadDetails={""} />

                <SpaceEvenly>
                    <SinglePurposeButton disabled={!companyLogo?.fileId} variant="contained" color="secondary" buttonText="Remove current logo" onClick={handleDeleteLogo} />
                </SpaceEvenly>
            </Paper>
        </SettingsWrapper>
    );
};
