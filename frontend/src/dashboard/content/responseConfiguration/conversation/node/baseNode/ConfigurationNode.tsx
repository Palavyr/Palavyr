import { makeStyles } from "@material-ui/core";
import { LineLink } from "@Palavyr-Types";
import classNames from "classnames";
import React, { useEffect, useState } from "react";
import { IPalavyrNode } from "../../Contracts";
import { SteppedLineTo } from "../../PalavyrNodeLines/SteppedLines";
import { NodeInterface } from "./NodeInterface";

const treelinkClassName = "tree-line-link";

type StyleProps = {
    buffer: number;
};

const useStyles = makeStyles(theme => ({
    treeItem: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        zIndex: 10,
    },
    treeBlockWrap: (props: StyleProps) => ({
        padding: `${props.buffer}rem ${props.buffer}rem ${props.buffer}rem ${props.buffer}rem`,
    }),
    treeRow: {
        display: "flex",
        flexDirection: "row",
    },
}));

export interface IConfigurationNode {
    currentNode: IPalavyrNode;
    pBuffer: number;
}

export const ConfigurationNode = ({ currentNode, pBuffer }: IConfigurationNode) => {
    const [loaded, setLoaded] = useState<boolean>(false);

    useEffect(() => {
        setLoaded(true);
        return () => setLoaded(false);
    }, []);

    const cls = useStyles({ buffer: pBuffer });
    return (
        <>
            <div className={classNames(treelinkClassName, cls.treeItem)}>
                <div className={cls.treeBlockWrap}>
                    <NodeInterface
                        currentNode={currentNode}
                        isRoot={currentNode.isRoot}
                        nodeType={currentNode.nodeType}
                        userText={currentNode.userText}
                        shouldPresentResponse={currentNode.shouldPresentResponse}
                        isMemberOfLeftmostBranch={currentNode.isMemberOfLeftmostBranch}
                        imageId={currentNode.imageId}
                        nodeId={currentNode.nodeId}
                        joinedChildReferenceString={currentNode.childNodeReferences.joinedReferenceString}
                        shouldDisableNodeTypeSelector={currentNode.shouldDisableNodeTypeSelector}
                        optionPath={currentNode.optionPath}
                    />
                </div>
                {currentNode.childNodeReferences.NotEmpty() && (
                    <div key={currentNode.nodeId} className={cls.treeRow}>
                        {currentNode.shouldRenderChildren ? (
                            currentNode.childNodeReferences.nodes.map(
                                (nextNode: IPalavyrNode, index: number): React.ReactNode => {
                                    return <ConfigurationNode key={[currentNode.nodeId, nextNode.nodeId, index.toString()].join("-")} currentNode={nextNode} pBuffer={pBuffer} />;
                                }
                            )
                        ) : (
                            <></>
                        )}
                    </div>
                )}
            </div>
            {loaded &&
                currentNode.lineMap.map((line: LineLink, index: number) => {
                    return <SteppedLineTo key={[line.from, index].join("-")} from={line.from} to={line.to} treeLinkClassName={treelinkClassName} />;
                })}
        </>
    );
};

