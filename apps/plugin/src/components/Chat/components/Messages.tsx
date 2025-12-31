import React from "react";
import { ConversationContent } from "./conversasion";
import { Message, MessageContent } from "./message";
import { Response } from "./response";
import { Loader } from "./loader";

const Messages: React.FC<{
  messages:any[];
  isLoading:boolean;
}> = ({ isLoading, messages }) => {
  return (
    <ConversationContent>
      {messages.map((msg, i) => (
        <Message key={i} from={msg.role}>
          <MessageContent>
            <Response>{msg.content}</Response>
          </MessageContent>
        </Message>
      ))}

      {isLoading && (
        <Message from="assistant">
          <MessageContent>
            <Loader />
          </MessageContent>
        </Message>
      )}
    </ConversationContent>
  );
};

export default Messages;
