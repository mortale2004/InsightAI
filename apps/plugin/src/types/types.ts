export type Region = "QA" | "Development";
export type FileType = "Form" | "Rule" | "Entity";

type ChildFileType = {
  fileName: string;
  fileType: FileType;
  fileContent: string;
};

export type appPromptType = {
  applicationName: string;
  regionName: Region;
  fileName: string;
  fileType: FileType;
  fileContent: string;
  childFiles?: ChildFileType[];
};
