### Team14_NT106.O22.ANTT
### GVHD: TS. Trần Hồng Nghi
###      TÊN ĐỒ ÁN: ỨNG DỤNG CỜ VUA 3D

#### 1. Mô tả đồ án
**1.	Giới thiệu**
Đồ án với đề tài "Game Cờ Vua 3D " được thực hiện nhằm mục đích ứng dụng kiến thức đã học trong môn Lập trình mạng căn bản vào thực tiễn. Qua đó, rèn luyện kỹ năng lập trình OOP, kỹ năng làm việc nhóm, kỹ năng tìm kiếm tài liệu và viết báo cáo.
Game Cờ Vua 3D cho phép người chơi thi đấu với nhau qua mạng LAN. Game được xây dựng với các tính năng cơ bản của cờ vua, đồng thời bổ sung thêm các chức năng như chat trong khi chơi, bảng xếp hạng người chơi và hệ thống kết bạn.

# Tổng Quan Giao Diện Trò Chơi

## 1. Giao Diện Đăng Nhập/Đăng Ký  
![Giao Diện Đăng Nhập/Đăng Ký](https://github.com/user-attachments/assets/4721249e-1c48-487f-8050-a6836b529b06)  

---

## 2. Giao Diện Trang Chủ Trò Chơi  
![Giao Diện Trang Chủ](https://github.com/user-attachments/assets/80f9140f-a9b5-4581-beac-bb8d24b2c82d)  


### Trang chủ chứa thanh điều khiển và 2 nút vào game:
- **Nút "Chơi với máy"**: Chuyển người chơi đến `Scenes GameAI`, nơi họ sẽ thi đấu với máy dựa trên thuật toán **Alpha-Beta**.
- **Nút "Chơi với người"**: Chuyển người chơi đến `Scenes GameUser`, yêu cầu kết nối **cùng một mạng Wi-Fi** để vào phòng chơi với bạn bè.

---

## 3. Giao Diện Trò Chơi Với Máy  
![Giao Diện Game Với Máy](https://github.com/user-attachments/assets/589df303-30ef-46f6-b978-e6d0349b1a71)  


### **Quy Tắc Khi Chơi Với Máy:**
- Người chơi sẽ điều khiển **quân cờ màu trắng** và luôn **đi trước**.  

### **Cách Thức Di Chuyển:**
1. **Chọn quân cờ**: Trỏ chuột vào quân cờ muốn di chuyển, ô vuông màu **đỏ** sẽ xuất hiện để đánh dấu quân cờ được chọn.
2. **Di chuyển quân cờ**: Các ô mà quân cờ có thể di chuyển đến sẽ có màu **xanh dương**. Người chơi click vào ô xanh dương để di chuyển quân cờ.
3. **Bắt quân đối thủ**:
   - Nếu trên bàn cờ xuất hiện **ô màu xanh lá** có chứa **quân cờ màu đen** của đối thủ, người chơi có thể di chuyển đến đó để **bắt quân**.
   - Khi di chuyển đến ô **màu xanh lá**, quân cờ đối thủ **sẽ biến mất** khỏi bàn cờ.
  
## 4. Giao Diện Trò Chơi Với Người  
### 4.1. Giao Diện Đợi Người Chơi Vào Phòng
![Giao Diện Game Với Máy](https://github.com/user-attachments/assets/de62fb8b-2017-4970-937f-a191ecc6f212)  
**Giao diện này xuất hiện khi người chơi nhấn nút “Tạo phòng”, màn hình sẽ lập tức chuyển sang giao diện này. Tại đây người tạo phòng sẽ đợi cho tới khi có người kết nối vào phòng bằng địa chỉ IP của phòng thì giao diện sẽ chuyển vào game.**
**Nếu người chơi không muốn đợi người chơi còn lại tham gia vào thì có thể nhấn nút “Cancel” để hủy phòng đang tạo và quay trở về màn hình sảnh chờ.**



---

