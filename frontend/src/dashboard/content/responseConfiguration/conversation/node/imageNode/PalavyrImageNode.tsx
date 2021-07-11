import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { PalavyrAutoComplete } from "@common/components/PalavyrAutoComplete";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { Dialog, DialogTitle, DialogContent, Typography, Divider } from "@material-ui/core";
import { ConvoNode, FileLink, SetState } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import { Upload } from "../../../uploadable/Upload";
import { CustomImage } from "./CustomImage";
import { IPalavyrLinkedList } from "../../Contracts";
import { useNodeInterfaceStyles } from "../../nodeInterfaceStyles";
import { PalavyrNodeBase } from "../PalavyrNode";
import { SessionStorage } from "localStorage/sessionStorage";

export class PalavyrImageNode extends PalavyrNodeBase {
    constructor(containerList: IPalavyrLinkedList, repository: PalavyrRepository, node: ConvoNode, setTreeWithHistory: (updatedTree: IPalavyrLinkedList) => void, leftmostBranch: boolean) {
        super(containerList, repository, node, setTreeWithHistory, leftmostBranch);
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
                        SessionStorage.setImageData(this.imageId, presignedUrl, fileLink.fileName, fileLink.fileId);
                    }
                }
            }, []);

            useEffect(() => {
                if (this.imageId !== null && this.imageId !== undefined) {
                    const imageData = SessionStorage.getImageData(this.imageId);
                    if (imageData !== null) {
                        setImageLink(imageData.presignedUrl);
                        setImageName(imageData.fileName);
                        setCurrentImageId(imageData.fileId);
                    } else {
                        loadImage();
                    }
                }
            }, [loadImage]);

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
                        console.log("WOw calling again!");
                        const presignedUrl = await this.repository.Configuration.Images.getSignedUrl(fileLink.link);
                        setImageLink(presignedUrl);
                        setImageName(fileLink.fileName);
                        setCurrentImageId(fileLink.fileId);
                        SessionStorage.setImageData(this.imageId, presignedUrl, fileLink.fileName, fileLink.fileId);
                    }
                }
            }, [this.imageId]);

            useEffect(() => {
                if (this.imageId !== null && this.imageId !== undefined) {
                    const imageData = SessionStorage.getImageData(this.imageId);
                    if (imageData !== null) {
                        setImageLink(imageData.presignedUrl);
                        setImageName(imageData.fileName);
                        setCurrentImageId(imageData.fileId);
                    }
                } else {
                    loadImage();
                }
            }, []);

            return (
                <Dialog fullWidth open={editorIsOpen} onClose={closeEditor}>
                    <DialogTitle>Edit a conversation node</DialogTitle>
                    <DialogContent>
                        {this.imageId === null
                            ? this.renderImageEditorWhenEmpty()({ closeEditor, currentImageId, setImageLink, setImageName })
                            : this.renderImageEditorWhenFull()({ closeEditor, imageName, imageLink, currentImageId, setImageLink, setImageName })}
                    </DialogContent>
                </Dialog>
            );
        };
    }

    public renderImageEditorWhenEmpty() {
        interface IRenderImageEditorWhenEmptyProps {
            closeEditor: () => void;
            currentImageId: string;
            setImageLink: SetState<string>;
            setImageName: SetState<string>;
        }
        return ({ closeEditor, currentImageId, setImageLink, setImageName }: IRenderImageEditorWhenEmptyProps) => {
            return (
                <>
                    <Typography align="center" variant="h6">
                        Upload an image
                    </Typography>
                    {this.renderImageUpload()({ closeEditor, currentImageId, setImageLink, setImageName, initialState: false })}
                </>
            );
        };
    }

    public renderImageEditorWhenFull() {
        interface ImageEditorWhenFullProps {
            closeEditor: () => void;
            imageName: string;
            imageLink: string;
            currentImageId: string;
            setImageLink: SetState<string>;
            setImageName: SetState<string>;
        }
        return ({ closeEditor, imageName, imageLink, currentImageId, setImageLink, setImageName }: ImageEditorWhenFullProps) => {
            return (
                <>
                    <CustomImage imageName={imageName} imageLink={imageLink} />
                    <Divider variant="fullWidth" style={{ marginBottom: "1rem" }} />
                    <Typography align="center" variant="h6">
                        Choose a new image
                    </Typography>
                    {this.renderImageUpload()({ closeEditor, currentImageId, setImageLink, setImageName, initialState: false })}
                    <Divider />
                </>
            );
        };
    }

    public renderImageUpload() {
        interface ImageUploadProps {
            closeEditor: () => void;
            currentImageId: string;
            setImageLink: SetState<string>;
            setImageName: SetState<string>;
            initialState: boolean;
        }
        return ({ closeEditor, currentImageId, setImageLink, setImageName, initialState = false }: ImageUploadProps) => {
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
                SessionStorage.clearImageFileLinks();
                setIsLoading(false);
                setSuccessOpen(true);
                closeEditor();
            };

            return (
                <>
                    <div className={cls.imageBlock}>{this.renderSelectFromExistingImages()({ currentImageId, setImageLink, setImageName })}</div>
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

    public renderSelectFromExistingImages() {
        interface SelectFromExistingImagesProps {
            currentImageId: string;
            setImageLink: SetState<string>;
            setImageName: SetState<string>;
        }
        return ({ currentImageId, setImageLink, setImageName }: SelectFromExistingImagesProps) => {
            const [options, setOptions] = useState<FileLink[] | null>(null);
            const [label, setLabel] = useState<string>("");

            const onChange = async (_: any, option: FileLink) => {
                await this.repository.Configuration.Images.savePreExistingImage(option.fileId, this.nodeId);
                setLabel(option.fileName);

                this.imageId = option.fileId;

                if (!option.isUrl) {
                    const imageData = SessionStorage.getImageData(this.imageId);
                    if (imageData !== null) {
                        setImageLink(imageData.presignedUrl);
                        setImageName(imageData.fileName);
                    } else {
                        const presignedUrl = await this.repository.Configuration.Images.getSignedUrl(option.link);
                        setImageLink(presignedUrl);
                        setImageName(option.fileName);
                        SessionStorage.setImageData(this.imageId, presignedUrl, option.fileName, "");
                    }
                }
                this.UpdateTree();
            };

            const groupGetter = (val: FileLink) => val.fileName;

            const setfilteredFileLinkOptions = (fileLinks: FileLink[]) => {
                const sortedOptions = sortByPropertyAlphabetical(groupGetter, fileLinks);
                const filteredOptions = sortedOptions.filter((link: FileLink) => {
                    return link.fileId !== currentImageId;
                });
                setOptions(filteredOptions);
            };

            const loadOptions = useCallback(async () => {
                const fileLinks = await this.repository.Configuration.Images.getImages();
                setfilteredFileLinkOptions(fileLinks);
                SessionStorage.setFileLinks(fileLinks);
            }, [currentImageId]);

            useEffect(() => {
                const fileLinks = SessionStorage.getFileLinks();
                if (fileLinks === null) {
                    loadOptions();
                } else {
                    setfilteredFileLinkOptions(fileLinks);
                }
            }, []);

            return (
                <PalavyrAccordian title="Select a file you've already uploaded" initialState={false}>
                    {options && <PalavyrAutoComplete label={label} options={options} shouldDisableSelect={false} onChange={onChange} getOptionLabel={(option) => option.fileName} />}
                </PalavyrAccordian>
            );
        };
    }
}
