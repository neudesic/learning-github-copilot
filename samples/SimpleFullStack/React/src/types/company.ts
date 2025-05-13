export type Company = {
  id: string;
  tpid?: string;
  accountName?: string;
  segment?: string;
  industry?: string;
  subSegment?: string;
  vertical?: string;
};

export const initialCompany: Company = {
  id: '-1',
  tpid: '',
  accountName: '',
  segment: '',
  subSegment: '',
  industry: '',
  vertical: '',
};
