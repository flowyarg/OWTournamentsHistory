﻿using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using OWTournamentsHistory.Api.GrpcServices.Helpers;
using OWTournamentsHistory.Api.Proto;
using OWTournamentsHistory.Api.Services;
using System.Diagnostics;

namespace OWTournamentsHistory.Api.GrpcServices
{
#if DEBUG
    [AllowAnonymous]
#endif
    public class PlayersHandlerService : PlayersHandler.PlayersHandlerBase
    {
        private readonly PlayersService _playersService;
        private readonly IMapper _mapper;
        private readonly ILogger<PlayersHandlerService> _logger;

        public PlayersHandlerService(PlayersService playersService, IMapper mapper, ILogger<PlayersHandlerService> logger)
        {
            _playersService = playersService;
            _mapper = mapper;
            _logger = logger;
        }

        public async override Task<PlayersGetManyResponse> GetMany(PlayersGetManyRequest request, ServerCallContext context)
        {
            return await Converters.WrapRpcCall(async () =>
            {
                var data = await _playersService.GetMany(request.Skip, request.Limit, context.CancellationToken);
                var result = new PlayersGetManyResponse();
                result.Players.AddRange(data.Select(_mapper.Map<Player>));
                Debug.WriteLine($"Response size: {result.CalculateSize()}");
                return result;
            });
        }

        public async override Task<PlayersGetResponse> Get(PlayersGetRequest request, ServerCallContext context)
        {
            return await Converters.WrapRpcCall(async () =>
            {
                var data = await _playersService.Get(request.Id, context.CancellationToken);
                var result = new PlayersGetResponse
                {
                    Player = _mapper.Map<Player>(data)
                };
                Debug.WriteLine($"Response size: {result.CalculateSize()}");
                return result;
            });
        }

        [Authorize("AdminScope")]
        public async override Task<PlayersAddResponse> Add(PlayersAddRequest request, ServerCallContext context)
        {
            return await Converters.WrapRpcCall(async () =>
            {
                var player = _mapper.Map<Contract.Model.Player>(request.Player);
                var playerId = await _playersService.Add(player, context.CancellationToken);
                var result = new PlayersAddResponse
                {
                    GeneratedId = playerId
                };
                Debug.WriteLine($"Response size: {result.CalculateSize()}");
                return result;
            });
        }

        [Authorize("AdminScope")]
        public async override Task<PlayersDeleteResponse> Delete(PlayersDeleteRequest request, ServerCallContext context)
        {
            return await Converters.WrapRpcCall(async () =>
            {
                await _playersService.Delete(request.Id, context.CancellationToken);
                return new PlayersDeleteResponse();
            });
        }

        [Authorize("AdminScope")]
        public async override Task<PlayersImportFromHtmlResponse> ImportFromHtml(PlayersImportFromHtmlRequest request, ServerCallContext context)
        {
            return await Converters.WrapRpcCall(async () =>
            {
                await _playersService.ImportFromHtml(request.Html, context.CancellationToken);
                return new PlayersImportFromHtmlResponse();
            });
        }
    }
}
