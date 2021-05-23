import React, { useState, useEffect, useCallback } from "react";
import { ConvoNode, SetState, ValueOptionDelimiter } from "@Palavyr-Types";
import { Dialog, DialogTitle, DialogContent, TextField, DialogActions, Typography, Divider } from "@material-ui/core";
import { MultiChoiceOptions } from "./MultiChoiceOptions";
import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { updateMultiTypeOption } from "../../nodeUtils/mutliOptionUtils";
import { updateSingleOptionType } from "../../nodeUtils/commonNodeUtils";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { CustomImage } from "./imageNode/CustomImage";
import { NodeImageUpload } from "./imageNode/ImageUpload";
import { PalavyrRepository } from "@api-client/PalavyrRepository";

export interface IConversationNodeEditor {
    modalState: boolean;
    setModalState: (state: boolean) => void;
    node: ConvoNode;
    currentImageId: string;
    imageName: string;
    setImageName: SetState<string>;
    imageLink: string;
    setImageLink: SetState<string>;
}

export const ConversationNodeEditor = ({ modalState, setModalState, node, currentImageId, imageName, setImageName, imageLink, setImageLink }: IConversationNodeEditor) => {
    const repository = new PalavyrRepository();

    const [options, setOptions] = useState<Array<string>>([]);
    const [textState, setText] = useState<string>("");
    const [switchState, setSwitchState] = useState<boolean>(true);

    const { nodeList, setNodes } = React.useContext(ConversationTreeContext);
    // const [imageLink, setImageLink] = useState<string>("");
    // const [imageName, setImageName] = useState<string>("");
    // const [currentImageId, setCurrentImageId] = useState<string>("");

    // const loadImage = useCallback(async () => {
    //     if (node.imageId !== null) {
    //         const fileLinks = await repository.Configuration.Images.getImages([node.imageId]);
    //         const fileLink = fileLinks[0];
    //         if (!fileLink.isUrl) {
    //             const presignedUrl = await repository.Configuration.Images.getSignedUrl(fileLink.link);
    //             setImageLink(presignedUrl);
    //             setImageName(fileLink.fileName);
    //             setCurrentImageId(fileLink.fileId);
    //         }
    //     }
    // }, [nodeList]);

    useEffect(() => {
        setText(node.text);
        if (node.isMultiOptionType && !isNullOrUndefinedOrWhitespace(node.valueOptions)) {
            setOptions(node.valueOptions.split(ValueOptionDelimiter));
        }
        // loadImage();
    }, [node]);

    const handleCloseModal = () => {
        setModalState(false);
        setText("");
    };

    const handleUpdateNode = (value: string, valueOptions: string[]) => {
        const updatedNode = { ...node };
        updatedNode.text = value;
        if (node.isMultiOptionType) {
            updateMultiTypeOption(updatedNode, nodeList, valueOptions, setNodes); // create new nodes and update the Database
        } else {
            updateSingleOptionType(updatedNode, nodeList, setNodes);
        }
        setText("");
    };

    const addMultiChoiceOptionsOnClick = () => {
        options.push("");
        setOptions(options);
        setSwitchState(!switchState);
    };

    return (
        <Dialog fullWidth open={modalState} onClose={handleCloseModal} aria-labelledby="form-dialog-title">
            <DialogTitle id="form-dialog-title">Edit a conversation node</DialogTitle>
            <DialogContent>
                {!node.isImageNode && (
                    <>
                        <TextField margin="dense" value={textState} multiline rows={4} onChange={(event) => setText(event.target.value)} id="question" label="Question or Information" type="text" fullWidth />
                        {node.isMultiOptionType && node.shouldShowMultiOption && (
                            <>
                                <MultiChoiceOptions options={options} setOptions={setOptions} switchState={switchState} setSwitchState={setSwitchState} addMultiChoiceOptionsOnClick={addMultiChoiceOptionsOnClick} />
                            </>
                        )}
                    </>
                )}
                {node.isImageNode && (
                    <>
                        {node.imageId === null ? (
                            <>
                                <Typography align="center" variant="h6">
                                    Upload an image or provide a url
                                </Typography>
                                <NodeImageUpload node={node} currentImageId={currentImageId} setImageLink={setImageLink} setImageName={setImageName} />
                            </>
                        ) : (
                            <>
                                <CustomImage imageName={imageName} imageLink={imageLink} />
                                <Divider variant="fullWidth" style={{ marginBottom: "1rem" }} />
                                <Typography align="center" variant="h6">
                                    Choose a new image
                                </Typography>
                                <NodeImageUpload node={node} currentImageId={currentImageId} setImageLink={setImageLink} setImageName={setImageName} initialState={false} />
                                <Divider />
                            </>
                        )}
                    </>
                )}
            </DialogContent>
            {!node.isImageNode && (
                <DialogActions>
                    <SaveOrCancel
                        position="right"
                        customSaveMessage="Node Text Updated"
                        customCancelMessage="Changes cancelled"
                        useSaveIcon={false}
                        saveText="Update Node Text"
                        onSave={async () => {
                            handleUpdateNode(textState, options);
                            handleCloseModal();
                            return true;
                        }}
                        onCancel={handleCloseModal}
                        timeout={200}
                    />
                </DialogActions>
            )}
        </Dialog>
    );
};
