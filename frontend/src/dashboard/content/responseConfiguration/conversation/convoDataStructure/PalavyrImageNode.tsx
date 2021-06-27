import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { PalavyrAutoComplete } from "@common/components/PalavyrAutoComplete";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { Dialog, DialogTitle, DialogContent, Typography, Divider } from "@material-ui/core";
import { ConvoNode, FileLink } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import { Upload } from "../../uploadable/Upload";
import { CustomImage } from "../nodes/nodeInterface/nodeEditor/imageNode/CustomImage";
import { IPalavyrLinkedList } from "./Contracts";
import { useNodeInterfaceStyles } from "./nodeInterfaceStyles";
import { PalavyrNode } from "./PalavyrNode";

export class PalavyrImageNode extends PalavyrNode {
    constructor(
        containerList: IPalavyrLinkedList,
        repository: PalavyrRepository,
        node: ConvoNode,
        nodeList: ConvoNode[],
        setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void,
        leftmostBranch: boolean
    ) {
        super(containerList, repository, node, nodeList, setTreeWithHistory, leftmostBranch);
    }

    public renderNodeFace() {
        return ({ openEditor }) => {
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

            return this.renderPalavyrNodeBody()({ openEditor, children: <CustomImage imageName={imageName} imageLink={imageLink} titleVariant="body1" /> });
        };
    }

    public renderNodeEditor() {
        return ({ editorIsOpen, closeEditor }) => {
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
                <Dialog fullWidth open={editorIsOpen} onClose={closeEditor}>
                    <DialogTitle>Edit a conversation node</DialogTitle>
                    <DialogContent>
                        {this.imageId === null
                            ? this.renderImageEditorWhenEmpty(closeEditor, currentImageId, setImageLink, setImageName)()
                            : this.renderImageEditorWhenFull(closeEditor, imageName, imageLink, currentImageId, setImageLink, setImageName)()}
                    </DialogContent>
                </Dialog>
            );
        };
    }

    public renderImageEditorWhenEmpty(closeEditor, currentImageId, setImageLink, setImageName) {
        return () => {
            return (
                <>
                    <Typography align="center" variant="h6">
                        Upload an image
                    </Typography>
                    {this.renderImageUpload(closeEditor, currentImageId, setImageLink, setImageName, false)()}
                </>
            );
        };
    }

    public renderImageEditorWhenFull(closeEditor: () => void, imageName, imageLink, currentImageId, setImageLink, setImageName) {
        return () => {
            return (
                <>
                    <CustomImage imageName={imageName} imageLink={imageLink} />
                    <Divider variant="fullWidth" style={{ marginBottom: "1rem" }} />
                    <Typography align="center" variant="h6">
                        Choose a new image
                    </Typography>
                    {this.renderImageUpload(closeEditor, currentImageId, setImageLink, setImageName, false)()}
                    <Divider />
                </>
            );
        };
    }

    public renderImageUpload(closeEditor: () => void, currentImageId, setImageLink, setImageName, initialState = false) {
        return () => {
            const cls = useNodeInterfaceStyles();
            const history = useHistory();
            const [uploadModal, setUploadModal] = useState(false);

            const { setIsLoading, setSuccessOpen, setSuccessText, planTypeMeta } = useContext(DashboardContext);
            useEffect(() => {
                if (planTypeMeta && !planTypeMeta.allowedImageUpload) {
                    history.push("/dashboard/please-subscribe");
                }
            }, [planTypeMeta]);

            const toggleModal = () => {
                setUploadModal(!uploadModal);
            };

            const fileSave = async (files: File[]) => {
                setIsLoading(true);
                const formData = new FormData();

                let result: FileLink[];
                if ((files.length = 1)) {
                    formData.append("files", files[0]);
                    result = await this.repository.Configuration.Images.saveSingleImage(formData);
                    setSuccessText("Image Uploaded");
                } else if (files.length > 1) {
                    files.forEach((file: File) => {
                        formData.append("files", file);
                    });
                    result = await this.repository.Configuration.Images.saveMultipleImages(formData);
                    setSuccessText("Images Uploaded");
                } else {
                    return;
                }

                await this.repository.Configuration.Images.savePreExistingImage(result[0].fileId, this.nodeId);
                setIsLoading(false);
                setSuccessOpen(true);
                closeEditor();
            };

            return (
                <>
                    <div className={cls.imageBlock}>{this.renderSelectFromExistingImages(currentImageId, setImageLink, setImageName)()}</div>
                    <Divider />
                    <div className={cls.imageBlock}>
                        {planTypeMeta && planTypeMeta.allowedImageUpload && (
                            <Upload
                                dropzoneType="area"
                                initialState={initialState}
                                modalState={uploadModal}
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

    public renderSelectFromExistingImages(currentImageId, setImageLink, setImageName) {
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
                this.setTreeWithHistory(this.palavyrLinkedList);
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
