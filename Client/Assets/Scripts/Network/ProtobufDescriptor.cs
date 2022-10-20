using System.Collections.Generic;
using Google.Protobuf;
using MyGame.Proto;

namespace Network
{
    public class ProtobufDescriptor
    {
        public delegate IMessage ParserDelegate(byte[] buffer, int offset, int size);
        
        private static Dictionary<PacketId, ParserDelegate> m_parsers = new()
        {
            {PacketId.UserRegisterRequest, (buffer,offset,size) => UserRegisterRequest.Descriptor.Parser.ParseFrom(buffer,offset,size)},
            {PacketId.UserRegisterResponse, (buffer,offset,size) => UserRegisterResponse.Descriptor.Parser.ParseFrom(buffer,offset,size)},
            {PacketId.UserLoginRequest, (buffer,offset,size) => UserLoginRequest.Descriptor.Parser.ParseFrom(buffer,offset,size)},
            {PacketId.UserLoginResponse, (buffer,offset,size) => UserLoginResponse.Descriptor.Parser.ParseFrom(buffer,offset,size)},
            {PacketId.UserCreateCharacterRequest, (buffer,offset,size) => UserCreateCharacterRequest.Descriptor.Parser.ParseFrom(buffer,offset,size)},
            {PacketId.UserCreateCharacterResponse, (buffer,offset,size) => UserCreateCharacterResponse.Descriptor.Parser.ParseFrom(buffer,offset,size)},
            {PacketId.UserGameEnterRequest, (buffer,offset,size) => UserGameEnterRequest.Descriptor.Parser.ParseFrom(buffer,offset,size)},
            {PacketId.UserGameEnterResponse, (buffer,offset,size) => UserGameEnterResponse.Descriptor.Parser.ParseFrom(buffer,offset,size)},
            {PacketId.UserGameLeaveRequest, (buffer,offset,size) => UserGameLeaveRequest.Descriptor.Parser.ParseFrom(buffer,offset,size)},
            {PacketId.UserGameLeaveResponse, (buffer,offset,size) => UserGameLeaveResponse.Descriptor.Parser.ParseFrom(buffer,offset,size)},
            {PacketId.MapCharacterEnterRequest, (buffer,offset,size) => MapCharacterEnterRequest.Descriptor.Parser.ParseFrom(buffer,offset,size)},
            {PacketId.MapCharacterEnterResponse, (buffer,offset,size) => MapCharacterEnterResponse.Descriptor.Parser.ParseFrom(buffer,offset,size)},
            {PacketId.MapCharacterLeaveResponse, (buffer,offset,size) => MapCharacterLeaveResponse.Descriptor.Parser.ParseFrom(buffer,offset,size)},
        };
        
        public static IMessage ParserFrom(PacketId id, byte[] buffer,int offset,int size) {
            if(m_parsers.TryGetValue(id, out ParserDelegate d)) {
                return d.Invoke(buffer, offset, size);
            }
            return null;
        }
    }
}