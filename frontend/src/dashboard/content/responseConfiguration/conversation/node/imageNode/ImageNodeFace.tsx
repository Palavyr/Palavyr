import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { SessionStorage } from "localStorage/sessionStorage";
import React, { useCallback, useEffect, useState } from "react";
import { PalavyrNodeBody } from "../baseNode/PalavyrNodeBody";
import { CustomImage } from "./CustomImage";

export interface ImageNodeFaceProps {
    openEditor: () => void;
    imageId?: string | null;
    repository: PalavyrRepository;
}

export const ImageNodeFace = ({ openEditor, imageId, repository }: ImageNodeFaceProps) => {
    const [imageLink, setImageLink] = useState<string>("");
    const [imageName, setImageName] = useState<string>("");
    const [currentImageId, setCurrentImageId] = useState<string>("");

    const loadImage = useCallback(async () => {
        if (imageId !== null && imageId !== undefined) {
            const fileLinks = await repository.Configuration.Images.getImages([imageId]);
            const fileLink = fileLinks[0];
            if (!fileLink.isUrl) {
                const presignedUrl = await repository.Configuration.Images.getSignedUrl(fileLink.s3Key, fileLink.fileId);
                setImageLink(presignedUrl);
                setImageName(fileLink.fileName);
                setCurrentImageId(fileLink.fileId);
            }
        }
    }, [imageId]);

    useEffect(() => {
        loadImage();
    }, [imageId]);

    return (
        <PalavyrNodeBody openEditor={openEditor} isImageNode>
            <CustomImage imageId={currentImageId} imageName={imageName} imageLink={imageLink} titleVariant="body1" />
        </PalavyrNodeBody>
    );
};
