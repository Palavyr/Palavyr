import { Conversation, ConvoNode, NodeIdentity } from "@Palavyr-Types";
import React, { useCallback, useState } from "react";
import { makeStyles, Card, CardContent, Typography } from "@material-ui/core";
import classNames from "classnames";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { useEffect } from "react";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { NodeCheckBox } from "./NodeCheckBox";
import { DataLogging } from "./nodeDebug/DataLogging";
import { ConversationNodeEditor } from "./nodeEditor/ConversationNodeEditor";
import { NodeTypeSelector } from "./nodeSelector/NodeTypeSelector";
import { filteredNodeTypeOptions } from "./nodeTypeFilter";
import { debugDataItems, debugNodeProperties } from "./nodeDebug/compileDebugData";
import { _handleMergeBackInOnClick } from "./nodeInterfaceCallbacks/_handleMergeBackInOnClick";
import { _showResponseInPdfCheckbox } from "./nodeInterfaceCallbacks/_showResponseInPdfCheckbox";
import { setNodeAsAnabranchMergePoint, _handleSetAsAnabranchMergePointClick } from "./nodeInterfaceCallbacks/_handleSetAsAnabranchMergePointClick";
import { _handleUnsetCurrentNodeType } from "./nodeInterfaceCallbacks/_handleUnsetCurrentNodeType";
import { _getParentNode } from "../nodeUtils/_coreNodeUtils";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { CustomImage } from "./nodeEditor/imageNode/CustomImage";
import { isNullOrUndefinedOrWhitespace } from "@common/utils";

type StyleProps = {
    nodeText: string;
    nodeType: string;
    checked: boolean;
    isDecendentOfSplitMerge: boolean;
    splitMergeRootSiblingIndex: number;
    debugOn: boolean;
    isImageNode: boolean;
};

const useStyles = makeStyles(() => ({
    root: (props: StyleProps) => ({
        minWidth: "275px",
        maxWidth: props.debugOn ? "600px" : "250px",
        minHeight: "350px",
        borderColor: props.nodeType === "" ? "red" : props.isDecendentOfSplitMerge && props.splitMergeRootSiblingIndex > 0 ? "purple" : "#54585A",
        borderWidth: props.nodeType === "" ? "5px" : props.isDecendentOfSplitMerge && props.splitMergeRootSiblingIndex > 0 ? "8px" : "2px",
        borderRadius: "3px",
        backgroundColor: "#C7ECEE",
    }),
    card: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "area",
    },
    bullet: {
        display: "inline-block",
        margin: "0 2px",
        transform: "scale(0.8)",
    },
    title: {
        fontSize: 14,
    },
    pos: {
        marginBottom: 12,
    },
    textCard: (props: StyleProps) => ({
        border: "1px solid gray",
        padding: "10px",
        textAlign: "center",
        color: props.nodeText === "Ask your question!" && !props.isImageNode ? "white" : "black",
        background: props.nodeText === "Ask your question!" && !props.isImageNode ? "red" : "white",
        "&:hover": {
            background: "lightgray",
            color: "black",
        },
    }),
    text: {
        margin: ".1rem",
    },
    formstyle: {
        fontSize: "12px",
        alignSelf: "bottom",
    },
    editorStyle: {
        fontSize: "12px",
        color: "lightgray",
    },
    formLabelStyle: (props: StyleProps) => ({
        fontSize: "12px",
        color: props.checked ? "black" : "gray",
    }),
    interfaceElement: {
        paddingBottom: "1rem",
    },
}));

export interface IConversationNodeInterface {
    node: ConvoNode;
    identity: NodeIdentity;
    reRender: () => void;
}

export const ConversationNodeInterface = ({ node, identity, reRender }: IConversationNodeInterface) => {
    const repository = new PalavyrRepository();
    const { setNodes, nodeTypeOptions, conversationHistory, historyTracker, conversationHistoryPosition, showDebugData } = React.useContext(ConversationTreeContext);

    const [modalState, setModalState] = useState<boolean>(false);
    const [mergeBoxChecked, setMergeBoxChecked] = useState<boolean>(false);
    const [anabranchMergeChecked, setAnabranchMergeChecked] = useState<boolean>(false);

    const [imageLink, setImageLink] = useState<string>("");
    const [imageName, setImageName] = useState<string>("");
    const [currentImageId, setCurrentImageId] = useState<string>("");

    const loadImage = useCallback(async () => {
        if (node.imageId !== null && node.imageId !== undefined) {
            const fileLinks = await repository.Configuration.Images.getImages([node.imageId]);
            const fileLink = fileLinks[0];
            if (!fileLink.isUrl) {
                const presignedUrl = await repository.Configuration.Images.getSignedUrl(fileLink.link);
                setImageLink(presignedUrl);
                setImageName(fileLink.fileName);
                setCurrentImageId(fileLink.fileId);
            }
        }
    }, []);

    const cls = useStyles({
        nodeType: node.nodeType,
        nodeText: node.text,
        checked: node.isCritical,
        isDecendentOfSplitMerge: identity.isDecendentOfSplitMerge,
        splitMergeRootSiblingIndex: identity.splitMergeRootSiblingIndex,
        debugOn: showDebugData,
        isImageNode: node.imageId !== null,
    });

    const dataItems = [...debugDataItems(identity), ...[{ anabranchMergeChecked: anabranchMergeChecked }, { conversationHistoryPosition: conversationHistoryPosition }, { modalState: modalState }, { mergeBoxChecked: mergeBoxChecked }]];
    const nodeProperties = debugNodeProperties(node);

    useEffect(() => {
        setMergeBoxChecked(identity.shouldCheckSplitMergeBox);
    }, [node]);

    useEffect(() => {
        if (identity.isAnabranchMergePoint) {
            setAnabranchMergeChecked(true);
        }
    }, []);

    useEffect(() => {
        loadImage();
    }, []);

    const showResponseInPdfCheckbox = (event: { target: { checked: boolean } }) => {
        const checked = event.target.checked;
        // _showResponseInPdfCheckbox(checked, node, rawNodeList, setNodes, conversationHistoryPosition, historyTracker, conversationHistory);
    };

    const handleMergeBackInOnClick = (event: { target: { checked: boolean } }) => {
        const checked = event.target.checked;
        // _handleMergeBackInOnClick(checked, node, rawNodeList, conversationHistoryPosition, historyTracker, conversationHistory, setNodes, setMergeBoxChecked, identity.nodeIdOfMostRecentSplitMergePrimarySibling);
    };

    const handleSetAsAnabranchMergePointClick = (event: { target: { checked: boolean } }) => {
        const checked = event.target.checked;
        // _handleSetAsAnabranchMergePointClick(checked, node, rawNodeList, identity.nodeIdOfMostRecentAnabranch, setAnabranchMergeChecked, setNodes);
    };

    const handleUnsetCurrentNodeType = () => {
        // _handleUnsetCurrentNodeType(node, rawNodeList, setNodes);
    };

    const selectionCallback = (node: ConvoNode, rawNodeList: Conversation, nodeIdOfMostRecentAnabranch: string): any => {
        // return setNodeAsAnabranchMergePoint(node, rawNodeList, nodeIdOfMostRecentAnabranch, setAnabranchMergeChecked);
    };

    return (
        <Card className={classNames(cls.root, node.nodeId)} variant="outlined">
            <CardContent className={cls.card}>
                {/* {showDebugData && nodeProperties && <DataLogging nodeProperties={nodeProperties} nodeChildren={node.nodeChildrenString} nodeId={node.nodeId} data={dataItems} />} */}
                <Typography className={cls.interfaceElement} variant={node.isRoot ? "h5" : "body1"} align="center">
                    {node.isRoot ? "Begin" : node.optionPath === "Continue" ? node.optionPath : "If " + node.optionPath}
                </Typography>
                <Card elevation={0} className={classNames(cls.interfaceElement, cls.textCard)} onClick={() => setModalState(true)}>
                    {node.isImageNode ? (
                        <CustomImage imageName={imageName} imageLink={imageLink} titleVariant="body1" />
                    ) : (
                        <Typography className={cls.text} variant="body2" component="span" noWrap={false}>
                            {node.text}
                        </Typography>
                    )}
                    <Typography align="center" className={cls.editorStyle} onClick={() => setModalState(true)}>
                        Click to Edit
                    </Typography>
                </Card>
                <NodeTypeSelector
                    nodeIdentity={identity}
                    nodeTypeOptions={filteredNodeTypeOptions(identity, nodeTypeOptions)}
                    nodeType={node.nodeType}
                    reRender={reRender}
                    shouldDisabledNodeTypeSelector={identity.shouldDisabledNodeTypeSelector}
                    selectionCallback={selectionCallback}
                />
                <ConversationNodeEditor key={node.nodeId} setImageLink={setImageLink} setImageName={setImageName} currentImageId={currentImageId} imageName={imageName} imageLink={imageLink} setModalState={setModalState} modalState={modalState} node={node} />

                {identity.shouldShowResponseInPdfOption && <NodeCheckBox label="Show response in PDF" checked={node.isCritical} onChange={showResponseInPdfCheckbox} />}
                {identity.shouldShowMergeWithPrimarySiblingBranchOption && <NodeCheckBox label="Merge with primary sibling branch" checked={!node.shouldRenderChildren} onChange={handleMergeBackInOnClick} />}
                {identity.shouldShowSetAsAnabranchMergePointOption && (
                    <NodeCheckBox disabled={node.isAnabranchType && node.isAnabranchMergePoint} label="Set as Anabranch merge point" checked={anabranchMergeChecked} onChange={handleSetAsAnabranchMergePointClick} />
                )}
                {identity.shouldShowUnsetNodeTypeOption && <SinglePurposeButton buttonText="Unset Node" variant="outlined" color="primary" onClick={handleUnsetCurrentNodeType} />}
                {identity.shouldShowSplitMergePrimarySiblingLabel && <Typography>This is the primary sibling. Branches will merge to this node.</Typography>}
                {identity.shouldShowAnabranchMergepointLabel && <Typography style={{ fontWeight: "bolder" }}>This is the Anabranch Merge Node</Typography>}
            </CardContent>
        </Card>
    );
};
