export type ConversationStatus = 'Open' | 'InProgress' | 'Waiting' | 'Closed';
export type MessageType = 'Text' | 'Image' | 'Audio' | 'Video' | 'Document';
export type MessageDirection = 'Inbound' | 'Outbound';

export interface MessageResponse {
  id: string;
  content: string;
  type: MessageType;
  direction: MessageDirection;
  sentAt: string;
  senderName?: string;
}

export interface ConversationResponse {
  id: string;
  status: ConversationStatus;
  channel: string;
  contactId: string;
  contactName: string;
  contactPhone: string;
  assignedAgentId?: string;
  assignedAgentName?: string;
  messages: MessageResponse[];
  lastMessageAt: string;
  lastMessagePreview: string;
  unreadCount: number;
  createdAt: string;
}

export interface ListConversationsResponse {
  items: ConversationResponse[];
  total: number;
  page: number;
  pageSize: number;
}
