import { EntryThumbContract } from '@/DataContracts/EntryThumbContract';
import { ReleaseEventContract } from '@/DataContracts/ReleaseEvents/ReleaseEventContract';
import { TagUsageForApiContract } from '@/DataContracts/Tag/TagUsageForApiContract';
import { WebLinkContract } from '@/DataContracts/WebLinkContract';
import { EntryStatus } from '@/Models/EntryStatus';
import { EventCategory } from '@/Models/Events/EventCategory';

// Corresponds to the ReleaseEventSeriesDetailsForApiContract record class in C#.
export interface ReleaseEventSeriesDetailsContract {
	additionalNames: string;
	category: EventCategory;
	deleted: boolean;
	description: string;
	events: ReleaseEventContract[];
	id: number;
	mainPicture?: EntryThumbContract;
	name: string;
	status: EntryStatus;
	tags: TagUsageForApiContract[];
	webLinks: WebLinkContract[];
}
