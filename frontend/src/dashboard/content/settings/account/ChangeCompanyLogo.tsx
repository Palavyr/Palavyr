import { ApiClient } from '@api-client/Client';
import { PalavyrAlert } from '@common/components/PalavyrAlert';
import { SinglePurposeButton } from '@common/components/SinglePurposeButton';
import { Divider, makeStyles, Paper, Typography } from '@material-ui/core';
import { Alert, AlertTitle } from '@material-ui/lab';
import { DropzoneArea } from 'material-ui-dropzone';
import * as React from 'react';
import { Dispatch, SetStateAction, useCallback, useEffect, useState } from 'react';

const useStyles = makeStyles(theme => ({
    paper: {
        backgroundColor: "#C7ECEE",
        padding: "2rem",
        margin: "1rem",
        width: "100%",
        display: "inline-block",
        justifyContent: "center",
        textAlign: "center",
        position: "relative"
    },
    dwrapper: {
        width: "100%",
        display: "flex",
        justifyContent: "center"
    },
    dropzone: {
        textAlign: "left",
        width: "100%",
    },
    title: {
        fontSize: "16pt"
    },
    snackbar: {
        backgroundColor: "#C7ECEE",
    },
    sep: {
        marginTop: "1rem",
        marginBottom: "1rem"
    },
    logoPreview: {
        verticalAlign: "middle",
        padding: "0.5rem",
        maxWidth: "450px",
        maxHeight: "450px",
        textAlign: "center",

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
        color: "white"
    },
    label: {
        color: "white"
    },
    alert: {},
    img: {
        maxWidth: "100%",
        maxHeight: "100%",
    },
    paperRoot: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "center"
    }
}))

export const ChangeLogoImage = () => {

    const [fileUpload, setFileUpload] = useState<File[]>([]);
    return <ChangeLogoImageInner fileUpload={fileUpload} setFileUpload={setFileUpload} />
}


interface ChangeLogoImageInner {
    fileUpload: File[];
    setFileUpload: Dispatch<SetStateAction<File[]>>;
}
const ChangeLogoImageInner = ({ fileUpload, setFileUpload }: ChangeLogoImageInner) => {
    var client = new ApiClient();
    const cls = useStyles();

    const [alertState, setAlertState] = useState<boolean>(false);
    const [companyLogo, setcompanyLogo] = useState<string>("");

    const alertMessage = {
        title: "Logo updated.",
        message: ""
    }

    const loadCompanyLogo = useCallback(async () => {
        var logoUri = (await client.Settings.Account.getCompanyLogo()).data as string;
        setcompanyLogo(logoUri);
    }, [])


    const handleFileChange = (files: File[]) => {
        setFileUpload(files);
    }

    const handleFileDelete = (file: File) => {
        setFileUpload([file]);
    }

    const handleFileSave = async () => {
        if (fileUpload !== null) {
            var formData = new FormData();
            formData.append('files', fileUpload[0])
            const dataUrl = (await client.Settings.Account.updateCompanyLogo(formData)).data as string;
            setcompanyLogo(dataUrl);
        }
        setFileUpload([]); // shouldn't this clear the chip
    }

    const getDropRejectMessage = (rejectedFile: File, acceptedFiles: string[], maxFileSize: number) => {
        let message = `File ${rejectedFile.name} was rejected. `;
        if (!acceptedFiles.includes(rejectedFile.type)) {
            message += "File type not supported. ";
        }
        const maxFileSizeInGb = maxFileSize / 1000000000 + " GB";
        if (rejectedFile.size > maxFileSize) {
            message += "File is too big. Size limit is " + maxFileSizeInGb + ". To upload a larger file we recommend using Octo.exe";
        }

        return message;
    };

    useEffect(() => {
        loadCompanyLogo();
        return () => {
            setFileUpload([])
        }
    }, [])

    const previewProps = { classes: { root: cls.previewChip, deleteIcon: cls.deleteIcon, label: cls.label } }

    return (
        <div style={{ width: "50%" }}>
            <Paper className={cls.paper}>

                <Alert className={cls.alert} severity={(companyLogo === "") ? "error" : "success"}>
                    <AlertTitle>
                        <Typography className={cls.title}>
                            {
                                (companyLogo === "")
                                    ? "Upload your company logo"
                                    : "Logo uploaded"
                            }
                        </Typography>
                    </AlertTitle>
                    <p>
                        Your company logo is placed into the top left area of each response PDF.
                    </p>
                    <p>
                        For the best results, use a square 250px by 250px png or svg image.
                    </p>
                </Alert>
                <div style={{ textAlign: "center", justifyContent: "center", display: "flex" }}>
                    <div style={{ display: "flex", flexDirection: "column", justifyContent: "center", marginTop: "1.4rem" }}>
                        <Typography variant="h5" style={{ marginBottom: "1rem" }}>Your Current Logo</Typography>
                        {
                            (companyLogo === "")
                                ? "Upload a company logo"
                                :
                                (
                                    <Paper className={cls.logoPreview} classes={{ root: cls.paperRoot }}>
                                        <img className={cls.img} src={companyLogo} />
                                    </Paper>
                                )
                        }
                    </div>
                </div>
                <Divider className={cls.sep} />
                <div className={cls.dropzone}>
                    <DropzoneArea
                        showAlerts={true}
                        onChange={handleFileChange}
                        onDelete={handleFileDelete}
                        dropzoneText={"Drag and drop a new image logo here or click"}
                        useChipsForPreview
                        showPreviewsInDropzone={false}
                        previewChipProps={previewProps}
                        acceptedFiles={['image/*']}
                        showPreviews={true}
                        maxFileSize={500000}
                        previewGridProps={{ item: { alignContent: "flex-start" }, container: { spacing: 2, direction: "row" }, }}
                        filesLimit={1}
                        previewText="Selected Files"
                        alertSnackbarProps={{ className: cls.snackbar }}
                        getDropRejectMessage={getDropRejectMessage}
                    />
                    <PalavyrAlert
                        alertState={alertState}
                        setAlertState={setAlertState}
                        useAlert
                        alertMessage={alertMessage}
                    />

                </div>
                <SinglePurposeButton
                    variant="contained"
                    color="primary"
                    buttonText="Upload and Save"
                    onClick={() => {
                        handleFileSave()
                    }}
                />
            </Paper>
        </div>

    )
}