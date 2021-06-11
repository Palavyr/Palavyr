import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { PalavyrAutoComplete } from "@common/components/PalavyrAutoComplete";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { Dialog, DialogTitle, DialogContent, Divider, Typography } from "@material-ui/core";
import { ConvoNode, FileLink, SetState } from "@Palavyr-Types";
import { useStyles } from "dashboard/content/demo/ColorOptions";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { findIndex } from "lodash";
import React, { useState, useCallback, useEffect, useContext } from "react";
import { useHistory } from "react-router-dom";
import { Upload } from "../../uploadable/Upload";
import { CustomImage } from "../nodes/nodeInterface/nodeEditor/imageNode/CustomImage";
import { NodeImageUpload } from "../nodes/nodeInterface/nodeEditor/imageNode/ImageUpload";
import { SelectFromExistingImages } from "../nodes/nodeInterface/nodeEditor/imageNode/SelectFromExistingImages";
import { NodeBody } from "./NodeBody";
import { useNodeInterfaceStyles } from "./nodeInterfaceStyles";
import { PalavyrNode } from "./PalavyrNode";

export class PalavyrImageNode extends PalavyrNode {
    constructor(containerList, repository, node, nodeList, rerender, leftMostBranch) {
        super(containerList, repository, node, nodeList, rerender, leftMostBranch);
    }

    renderNodeFace(setModalState: SetState<boolean>) {
        return () => {
            const [imageLink, setImageLink] = useState<string>("");
            const [imageName, setImageName] = useState<string>("");
            const [currentImageId, setCurrentImageId] = useState<string>("");

            const loadImage = useCallback(async () => {
                if (this.imageId !== null && this.imageId !== undefined) {
                    const fileLinks = await this.repository.Configuration.Images.getImages([this.imageId]);
                    const fileLink = fileLinks[0];
                    if (!fileLink.isUrl) {
                        const presignedUrl = await this.repository.Configuration.Images.getSignedUrl(fileLink.link);
                        setImageLink(presignedUrl);
                        setImageName(fileLink.fileName);
                        setCurrentImageId(fileLink.fileId);
                    }
                }
            }, [this.palavyrLinkedList]);

            useEffect(() => {
                loadImage();
            }, [this.palavyrLinkedList]);

            return (
                <NodeBody setModalState={setModalState}>
                    <CustomImage imageName={imageName} imageLink={imageLink} titleVariant="body1" />
                </NodeBody>
            );
        };
    }

    protected renderNodeEditor(modalState, setModalState) {
        return () => {
            const [imageLink, setImageLink] = useState<string>("");
            const [imageName, setImageName] = useState<string>("");
            const [currentImageId, setCurrentImageId] = useState<string>("");

            const loadImage = useCallback(async () => {
                if (this.imageId !== null && this.imageId !== undefined) {
                    const fileLinks = await this.repository.Configuration.Images.getImages([this.imageId]);
                    const fileLink = fileLinks[0];
                    if (!fileLink.isUrl) {
                        const presignedUrl = await this.repository.Configuration.Images.getSignedUrl(fileLink.link);
                        setImageLink(presignedUrl);
                        setImageName(fileLink.fileName);
                        setCurrentImageId(fileLink.fileId);
                    }
                }
            }, [this.palavyrLinkedList]);

            useEffect(() => {
                loadImage();
            }, [this.palavyrLinkedList]);

            const handleCloseModal = () => {
                setModalState(false);
            };

            return (
                <Dialog fullWidth open={modalState} onClose={handleCloseModal}>
                    <DialogTitle>Edit a conversation node</DialogTitle>
                    <DialogContent>
                        {this.imageId === null
                            ? this.renderImageEditorWhenEmpty(setModalState, currentImageId, setImageLink, setImageName)
                            : this.renderImageEditorWhenFull(setModalState, imageName, imageLink, currentImageId, setImageLink, setImageName)}
                    </DialogContent>
                </Dialog>
            );
        };
    }

    protected renderImageEditorWhenEmpty(setModalState: SetState<boolean>, currentImageId, setImageLink, setImageName) {
        return () => {
            return (
                <>
                    <Typography align="center" variant="h6">
                        Upload an image
                    </Typography>
                    {this.renderImageUpload(setModalState,  currentImageId, setImageLink, setImageName, false)}
                </>
            );
        };
    }

    protected renderImageEditorWhenFull(setModalState: SetState<boolean>, imageName, imageLink, currentImageId, setImageLink, setImageName) {
        return () => {
            return (
                <>
                    <CustomImage imageName={imageName} imageLink={imageLink} />
                    <Divider variant="fullWidth" style={{ marginBottom: "1rem" }} />
                    <Typography align="center" variant="h6">
                        Choose a new image
                    </Typography>
                    {this.renderImageUpload(setModalState,  currentImageId, setImageLink, setImageName, false)}
                    <Divider />
                </>
            );
        };
    }

    private renderImageUpload(setModalState: SetState<boolean>, currentImageId, setImageLink, setImageName, initialState = false) {
        return () => {
            const cls = useNodeInterfaceStyles();
            const { setIsLoading, setSuccessOpen, setSuccessText, planTypeMeta } = useContext(DashboardContext);
            const history = useHistory();

            useEffect(() => {
                if (planTypeMeta && !planTypeMeta.allowedImageUpload) {
                    history.push("/dashboard/please-subscribe");
                }
            }, [planTypeMeta]);

            const repository = new PalavyrRepository();
            const [modal, setModal] = useState(false);

            const toggleModal = () => {
                setModal(!modal);
            };

            const fileSave = async (files: File[]) => {
                setIsLoading(true);
                const formData = new FormData();

                let result: FileLink[];
                if ((files.length = 1)) {
                    formData.append("files", files[0]);
                    result = await repository.Configuration.Images.saveSingleImage(formData);
                    setSuccessText("Image Uploaded");
                } else if (files.length > 1) {
                    files.forEach((file: File) => {
                        formData.append("files", file);
                    });
                    result = await repository.Configuration.Images.saveMultipleImages(formData);
                    setSuccessText("Images Uploaded");
                } else {
                    return;
                }

                await repository.Configuration.Images.savePreExistingImage(result[0].fileId, this.nodeId);
                setIsLoading(false);
                setSuccessOpen(true);
                setModalState(false);
            };

            return (
                <>
                    <div className={cls.imageBlock}>
                        {this.renderSelectFromExistingImages( currentImageId, setImageLink, setImageName)}
                    </div>
                    <Divider />
                    <div className={cls.imageBlock}>
                        {planTypeMeta && planTypeMeta.allowedImageUpload && (
                            <Upload
                                dropzoneType="area"
                                initialState={initialState}
                                modalState={modal}
                                toggleModal={() => toggleModal()}
                                handleFileSave={(files: File[]) => fileSave(files)}
                                summary="Upload a file."
                                buttonText="Upload"
                                uploadDetails={<Typography>Upload an image, pdf, or other document you wish to share with your users</Typography>}
                                acceptedFiles={["image/png", "image/jpg"]}
                            />
                        )}
                    </div>
                </>
            );
        };
    }

    private renderSelectFromExistingImages( currentImageId, setImageLink, setImageName) {
        return () => {
            const [options, setOptions] = useState<FileLink[] | null>(null);
            const [label, setLabel] = useState<string>("");

            const onChange = async (_: any, option: FileLink) => {

                const convoNode = await this.repository.Configuration.Images.savePreExistingImage(option.fileId, this.nodeId);
                setLabel(option.fileName);

                this.imageId = option.fileId;

                if (!option.isUrl) {
                    const presignedUrl = await this.repository.Configuration.Images.getSignedUrl(option.link);
                    setImageLink(presignedUrl);
                    setImageName(option.fileName);
                }
                // setNodes(nodeList);
            };

            const groupGetter = (val: FileLink) => val.fileName;

            const loadOptions = useCallback(async () => {
                const fileLinks = await this.repository.Configuration.Images.getImages();
                const sortedOptions = sortByPropertyAlphabetical(groupGetter, fileLinks);
                const filteredOptions = sortedOptions.filter((link: FileLink) => {
                    return link.fileId !== currentImageId;
                });
                setOptions(filteredOptions);
            }, [currentImageId]);

            useEffect(() => {
                loadOptions();
            }, [currentImageId]);

            return (
                <PalavyrAccordian title="Select a file you've already uploaded" initialState={false}>
                    {options && <PalavyrAutoComplete label={label} options={options} shouldDisableSelect={false} onChange={onChange} getOptionLabel={(option) => option.fileName} />}
                </PalavyrAccordian>
            );
        };
    }
}
