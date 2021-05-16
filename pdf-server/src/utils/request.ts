import { RequestBody } from "@Palavyr-Types"
import { Request } from 'express';


export const unpackRequest = (req: Request): RequestBody => {
	return {
		s3bucket: req.body.Bucket,
		s3key: req.body.Key,
		html: req.body.Html,
		identifier: req.body.Id,
		paper: req.body.Paper
	}
}