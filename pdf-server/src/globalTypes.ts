import { CreateOptions } from 'html-pdf';

export type ResponseBody = {
	FullPath: string;
	TempDirectory: string;
	FileNameWithExtension: string;
	FileStem: string;
};

export type RequestBody = {
	s3bucket: string;
	s3key: string;
	html: string;
	identifier: string;
	paper: CreateOptions;
}