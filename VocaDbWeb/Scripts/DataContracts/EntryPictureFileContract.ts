import { EntryType } from '@/Models/EntryType';

export interface EntryPictureFileContract {
	entryType: EntryType;
	fileName: string;
	id: number;
	mime: string;
	name: string;
	thumbUrl: string;
}
