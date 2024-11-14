// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

// Hợp đồng lưu trữ địa chỉ của tám người chơi
contract PlayerGame {
    address[8] public players;
    mapping(address => bool) public hasWithdrawnOneEther; // Theo dõi nếu một người chơi đã rút đúng 1 ETH

    // Constructor khởi tạo địa chỉ của 8 người chơi
    constructor(
        address _player1,
        address _player2,
        address _player3,
        address _player4,
        address _player5,
        address _player6,
        address _player7,
        address _player8
    ) {
        players[0] = _player1;
        players[1] = _player2;
        players[2] = _player3;
        players[3] = _player4;
        players[4] = _player5;
        players[5] = _player6;
        players[6] = _player7;
        players[7] = _player8;
    }

    // Hàm lấy địa chỉ của một người chơi
    function getPlayer(uint index) external view returns (address) {
        require(index < 8, "Invalid player index");
        return players[index];
    }

    // Hàm cho phép bất kỳ người chơi nào rút toàn bộ số dư của hợp đồng
    function withdrawAll() external {
        bool isPlayer = false;
        for (uint i = 0; i < 8; i++) {
            if (msg.sender == players[i]) {
                isPlayer = true;
                break;
            }
        }
        require(isPlayer, "You are not a player");

        payable(msg.sender).transfer(address(this).balance); // Chuyển toàn bộ số dư cho người chơi
    }

    // Hàm cho phép mỗi người chơi rút đúng 1 ETH và chỉ một lần
    function withdrawOneEther() external {
        bool isPlayer = false;
        for (uint i = 0; i < 8; i++) {
            if (msg.sender == players[i]) {
                isPlayer = true;
                break;
            }
        }
        require(isPlayer, "You are not a player");
        require(
            !hasWithdrawnOneEther[msg.sender],
            "You have already withdrawn 1 ETH"
        );
        require(
            address(this).balance >= 1 ether,
            "Insufficient balance in contract"
        );

        hasWithdrawnOneEther[msg.sender] = true; // Đánh dấu người chơi đã rút đúng 1 ETH
        payable(msg.sender).transfer(1 ether); // Chuyển đúng 1 ETH cho người chơi
    }

    // Nhận Ether vào hợp đồng
    receive() external payable {}

    // Hàm trả về số dư của hợp đồng
    function getBalance() external view returns (uint) {
        return address(this).balance; // Trả về số dư hiện tại của hợp đồng
    }
}

// Hợp đồng Factory để tạo ra các hợp đồng PlayerGame
contract PlayerGameFactory {
    PlayerGame[] public games;

    // Hàm tạo hợp đồng PlayerGame mới
    function createGame(
        address _player1,
        address _player2,
        address _player3,
        address _player4,
        address _player5,
        address _player6,
        address _player7,
        address _player8
    ) external {
        PlayerGame newGame = new PlayerGame(
            _player1,
            _player2,
            _player3,
            _player4,
            _player5,
            _player6,
            _player7,
            _player8
        );
        games.push(newGame); // Thêm hợp đồng mới vào danh sách
    }

    // Hàm lấy danh sách các hợp đồng đã tạo
    function getGames() external view returns (PlayerGame[] memory) {
        return games;
    }

    // Hàm lấy địa chỉ hợp đồng gần nhất có sự tham gia của player1
    function getLastGameForPlayer1(
        address _player1
    ) external view returns (address) {
        for (uint i = games.length; i > 0; i--) {
            PlayerGame game = games[i - 1];
            if (game.getPlayer(0) == _player1) {
                // Kiểm tra nếu player1 khớp
                return address(game); // Trả về địa chỉ hợp đồng gần nhất cho player1
            }
        }
        revert("No game found for the given player.");
    }
}
